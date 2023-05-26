using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    public GameObject CrowPrefab;
    public GameObject BasicCropPrefab;

    public float WaggleScoreMultiplier = 1f;
    [Range(0, 1)] public float ArmExtensionImportance = 0.5f;
    public float DebugSpookAmount = 0.2f;
    public float SpawnIntervalMultiplier = 1f;

    public float SpawnDistance = 100f;

    public GameObject Player;
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Head;

    public GameObject LeftHandModelPrefabScarecrow;
    public GameObject RightHandModelPrefabScarecrow;
    public GameObject LeftHandModelPrefabFarmer;
    public GameObject RightHandModelPrefabFarmer;

    // --- Player attributes
    public Transform BeltItems;
    public Transform FarmerBeltItems;
    public Transform ScarecrowBeltItems;
    BeltObject leftHeldObject;
    BeltObject rightHeldObject;

    // --- Game attributes

    public WaggleGameStateChanger PortalToShop;
    public WaggleGameStateChanger PortalToFarm;
    public WaggleGameStateChanger RoundStarter;

    public GameStates GameState = GameStates.Farm;
    float roundTimer = 0f; // seconds
    public float RoundEndTime = 5f;

    public Dictionary<Plant, int> Resources = new Dictionary<Plant, int>();

    public Plant[] CSDPlants;
    public GameObject[] CSDPrefabs;
    public float[] CSDWeights;
    public WeightedRandom<GameObject> birdSpawner;

    public static float LeftArmExtension;
    public static float RightArmExtension;
    public static Vector3 LeftHandLastPos;
    public static Vector3 RightHandLastPos;
    public static float LeftHandWaggleScore;
    public static float RightHandWaggleScore;
    public static float GlobalWaggleMultiplier;

    // --- Farm island attributes

    public Transform FarmPlayerSpot;

    public Whiteboard BodySizeWhiteboard;

    public GameObject SeedBagPumpkin;
    public GameObject SeedBagPepper;

    public ScarecrowGun NoiseGun;
    public ScarecrowGun PepperSpray;

    public float BodySize = 64f;
    public float EffectiveBodySize = 8f; // equal to sqrt(BodySize)
    public float PumpkinArmor = 0f;

    public Transform LiveCrops;
    public Transform DeadCrops;
    public Transform Birds;

    public GameObject FakeScarecrow;

    // --- Shop island attributes

    public Transform ShopPlayerSpot;

    public Whiteboard ShopWhiteboard;
    public Whiteboard ResourcesWhiteboard;
    public Whiteboard CheckoutWhiteboard;

    public GameObject UpgradeUnlockPlantPumpkin;
    public GameObject UpgradeUnlockPlantPepper;

    public GameObject UpgradeUnlockToolNoiseGun;
    public GameObject UpgradeUnlockToolPepperSpray;

    public GameObject UpgradePumpkinHead;
    public GameObject UpgradeCapsaicinRefill;

    // --- FMOD

    public StudioEventEmitter ActionMusic;
    public StudioEventEmitter ShopMusic;
    public StudioEventEmitter SeagullAmbience;
    public int IntensityThreshold1;
    public int IntensityThreshold2;

    // --- Technical

    public bool VRFallback = false;
    public Canvas UICanvasPrefab;
    public SteamVR_Action_Single UseHeldObject;

    // ---

    public enum GameStates {
        Farm, // Allow player to plant seeds
        Shop, // Allow player to buy upgrades
        Scarecrow, // Spawn birds
        ScarecrowEnd, // No birds spawn; move to next game phase once all birds have been scared away
    }

    public int Cycle;
    public CycleSettings[] Cycles;
    public Light DirectionalLight;
    public Material OceanMaterial;

    private void Awake() {
        Instance = this;
        AddResource(Wheat, 5);
    }

    private void Start() {
        ChangeCycle();
        ChangeBodySize(0f);
        ActionMusic.Stop();
        SeagullAmbience.Stop();
        ShopMusic.Stop();
        Canvas canv = Instantiate(UICanvasPrefab);
        if (VRFallback) {
            Transform noSteamVRFallbackObjects = Player.transform.Find("NoSteamVRFallbackObjects");
            RightHand = noSteamVRFallbackObjects.Find("FallbackHand").gameObject;
            Head = noSteamVRFallbackObjects.Find("FallbackObjects").gameObject;
        } else {
            Transform steamVRObjects = Player.transform.Find("SteamVRObjects");
            LeftHand = steamVRObjects.Find("LeftHand").gameObject;
            RightHand = steamVRObjects.Find("RightHand").gameObject;
            Head = steamVRObjects.Find("VRCamera").gameObject;
        }
        canv.worldCamera = Head.GetComponent<Camera>();
        canv.planeDistance = 0.5f;

        UseHeldObject.AddOnUpdateListener(UseHeldItemLeft, SteamVR_Input_Sources.LeftHand);
        UseHeldObject.AddOnUpdateListener(UseHeldItemRight, SteamVR_Input_Sources.RightHand);
    }

    public Plant Wheat;
    private void Update() {
        calculateWaggle();
        BeltItems.position = Head.transform.position - new Vector3(0f, 0.5f, 0f);
        if (Input.GetKeyDown("space")) {
            for (int i = 0; i < Birds.transform.childCount; i++) {
                Birds.transform.GetChild(i).GetComponent<Bird>().Waggle += DebugSpookAmount;
            }
        }
        if (GameState == GameStates.Scarecrow) {
            roundTimer += Time.deltaTime;
            if (roundTimer >= RoundEndTime || Input.GetKeyDown("c")) {
                ChangeGameState(GameStates.ScarecrowEnd);
            }
        } else if (GameState == GameStates.ScarecrowEnd) {
            if (Birds.transform.childCount == 0) {
                onCycleEnd();
                ChangeCycle();
                ChangeGameState(GameStates.Farm);
            }
        } else if (GameState == GameStates.Farm) {
            FakeScarecrow.SetActive(false);
            if (Input.GetKeyDown("m")) {
                ChangeGameState(GameStates.Shop);
            } else if (Input.GetKeyDown("c")) {
                ChangeGameState(GameStates.Scarecrow);
            }
        } else if (GameState == GameStates.Shop) {
            FakeScarecrow.SetActive(true);
            if (Input.GetKeyDown("m")) {
                ChangeGameState(GameStates.Farm);
            }
        }
        if (Input.GetKeyDown("l")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit)) {
                Planter planter = rayHit.transform.gameObject.GetComponent<Planter>();
                if (planter != null) {
                    planter.Crop.ReceiveSeed(Wheat);
                }
                WaggleGameStateChanger wgsc = rayHit.transform.gameObject.GetComponent<WaggleGameStateChanger>();
                if (wgsc != null) {
                    wgsc.OnTrigger();
                }
            }
        }
    }

    public void ChangeGameState(GameStates gs) {
        GameState = gs;
        if (GameState == GameStates.Scarecrow) {
            RoundStarter.gameObject.SetActive(false);
            PortalToFarm.gameObject.SetActive(false);
            PortalToShop.gameObject.SetActive(false);
            FarmerBeltItems.gameObject.SetActive(false);
            ScarecrowBeltItems.gameObject.SetActive(true);
            ActionMusic.Play();
            SeagullAmbience.Play();
            NoiseGun.Refill();
            roundTimer = 0f;
            StartCoroutine(spawnBird());
            if (!VRFallback) {
                LeftHand.GetComponent<Hand>().SetRenderModel(LeftHandModelPrefabScarecrow);
                RightHand.GetComponent<Hand>().SetRenderModel(RightHandModelPrefabScarecrow);
            }
            //generateBirdSpawner();
            Debug.Log("Scarecrow phase beginning");
        } else if (GameState == GameStates.ScarecrowEnd) {
            Debug.Log("Scarecrow phase ending...");
        } else if (GameState == GameStates.Farm) {
            RoundStarter.gameObject.SetActive(true);
            PortalToFarm.gameObject.SetActive(false);
            PortalToShop.gameObject.SetActive(true);
            ActionMusic.Stop();
            SeagullAmbience.Stop();
            ShopMusic.Stop();
            FakeScarecrow.SetActive(false);
            Player.transform.parent = FarmPlayerSpot;
            Player.transform.localPosition = Vector3.zero;
            Debug.Log("Entering farm phase");
            FarmerBeltItems.gameObject.SetActive(true);
            ScarecrowBeltItems.gameObject.SetActive(false);
            if (!VRFallback) {
                LeftHand.GetComponent<Hand>().SetRenderModel(LeftHandModelPrefabFarmer);
                RightHand.GetComponent<Hand>().SetRenderModel(RightHandModelPrefabFarmer);
            }
        } else if (GameState == GameStates.Shop) {
            RoundStarter.gameObject.SetActive(false);
            PortalToFarm.gameObject.SetActive(true);
            PortalToShop.gameObject.SetActive(false);
            ShopMusic.Play();
            Player.transform.parent = ShopPlayerSpot;
            Player.transform.localPosition = Vector3.zero;
            FarmerBeltItems.gameObject.SetActive(false);
            ScarecrowBeltItems.gameObject.SetActive(false);
            Debug.Log("Moving to shop");
        }
    }

    public void ChangeBodySize(float d)
    {
        BodySize += d;
        EffectiveBodySize = 3 * Mathf.Sqrt(BodySize);
        if (BodySize < 0)
        {
            Player.GetComponent<Animator>().SetBool("IsDead", true);
            BodySizeWhiteboard.Text.SetText("YOU DIED");
        } else
        {
            BodySizeWhiteboard.Text.SetText("Body size:\n" + BodySize);
        }
    }

    public void ChangeCycle()
    {
        Cycle++;
        if (Cycle == Cycles.Length)
        {
            Cycle = 0;
        }
        CycleSettings cs = Cycles[Cycle];
        DirectionalLight.intensity = cs.LightIntensity;
        OceanMaterial.SetColor("_BaseColor", cs.OceanColor);
        RenderSettings.skybox = cs.Skybox;
    }

    public void DebugToggleMode() {
        switch (GameState) {
            case GameStates.Scarecrow: ChangeGameState(GameStates.ScarecrowEnd); break;
            case GameStates.ScarecrowEnd: break;
            case GameStates.Farm: ChangeGameState(GameStates.Scarecrow); break;
            case GameStates.Shop: ChangeGameState(GameStates.Farm); break;
        }
    }

    // value should be between 0 and 1
    public void UseHeldItem(float triggerValue, bool rightHand) {
        BeltObject obj = rightHand ? rightHeldObject : leftHeldObject;
        if (obj == null) { return; }
        obj.Use(triggerValue);
    }

    // ghdgskjsdhfjg
    private void generateBirdSpawner() {
        Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>();
        for (int i = 0; i < LiveCrops.childCount; i++) {
            for (int j = 0; j < CSDPlants.Length; j++)
            {
                if (LiveCrops.transform.GetChild(i).GetComponent<Crop>().PlantType == CSDPlants[j])
                {
                    if (!dict.ContainsKey(CSDPrefabs[j]))
                    {
                        dict[CSDPrefabs[j]] = 0;
                    }
                    dict[CSDPrefabs[j]] += CSDWeights[j];
                }
            }
        }
        float[] weights = new float[dict.Count];
        GameObject[] values = new GameObject[dict.Count];
        { 
            int i = 0;
            foreach (var kvp in dict) {
                weights[i] = kvp.Value;
                values[i] = kvp.Key;
                i++;
            }
        }
        birdSpawner = new WeightedRandom<GameObject>(weights, values);
    }

    // --- SteamVR input methods. Listeners are added for these in Start.
    public void DebugToggleMode(SteamVR_Action_Boolean action, SteamVR_Input_Sources source) {
        DebugToggleMode();
    }
    private void UseHeldItemLeft(SteamVR_Action_Single action, SteamVR_Input_Sources source, float newAxis, float newDelta) {
        UseHeldItem(newAxis, false);
    }
    private void UseHeldItemRight(SteamVR_Action_Single action, SteamVR_Input_Sources source, float newAxis, float newDelta) {
        UseHeldItem(newAxis, true);
    }
    // ---

    public void OnBeltObjectAttached(BeltObject obj, Hand hand) {
        if (hand.handType == SteamVR_Input_Sources.LeftHand) {
            leftHeldObject = obj;
        } else if (hand.handType == SteamVR_Input_Sources.RightHand) {
            rightHeldObject = obj;
        } else {
            Debug.Log("iurfrehrg");
        }
    }

    public void OnBeltObjectDetached(Hand hand) {
        if (hand.handType == SteamVR_Input_Sources.LeftHand) {
            leftHeldObject = null;
        } else if (hand.handType == SteamVR_Input_Sources.RightHand) {
            rightHeldObject = null;
        }
    }

    public void AddResource(Plant PlantType, int amount) {
        int n = Resources.ContainsKey(PlantType) ? Resources[PlantType] : 0;
        Resources[PlantType] = n + amount;
        ResourcesWhiteboard.LoadTextFromResources(Resources);
        Debug.Log("Got "+amount+" "+PlantType.name);
    }

    public bool AttemptPurchase(Dictionary<Plant, int> cost) {
        bool valid = true;
        foreach (var kvp in cost) {
            if (!Resources.ContainsKey(kvp.Key) || Resources[kvp.Key] <= kvp.Value) {
                valid = false;
            }
        }
        if (valid) {
            foreach (var kvp in cost) {
                Resources[kvp.Key] -= kvp.Value;
            }
        }
        ResourcesWhiteboard.LoadTextFromResources(Resources);
        return valid;
    }

    public void CheckoutText(string text)
    {
        CheckoutWhiteboard.Text.SetText(text);
        StartCoroutine(resetCheckoutText());
    }

    private IEnumerator resetCheckoutText()
    {
        yield return new WaitForSeconds(4);
        CheckoutWhiteboard.Text.SetText("Please place your items\nin the bagging area\nv");
        yield return null;
    }

    private void calculateWaggle() {
        RightHandWaggleScore = Mathf.Clamp((RightHandLastPos - RightHand.transform.position).magnitude, 0, 0.03f)
                             * (RightArmExtension * ArmExtensionImportance + 1 - ArmExtensionImportance);
        RightHandLastPos = RightHand.transform.position;
        RightArmExtension = (Head.transform.position - RightHand.transform.position).magnitude;
        if (!VRFallback) {
            LeftHandWaggleScore = Mathf.Clamp((LeftHandLastPos - LeftHand.transform.position).magnitude, 0f, 0.03f)
                                * (LeftArmExtension * ArmExtensionImportance + 1 - ArmExtensionImportance);
            LeftArmExtension = (Head.transform.position - LeftHand.transform.position).magnitude;
            LeftHandLastPos = LeftHand.transform.position;
            GlobalWaggleMultiplier = (RightHandWaggleScore + LeftHandWaggleScore) * WaggleScoreMultiplier;
        } else {
            GlobalWaggleMultiplier = RightHandWaggleScore * 2 * WaggleScoreMultiplier;
        }
    }

    private void onCycleEnd() {
        for (int i = 0; i < LiveCrops.transform.childCount; i++) {
            Crop crop = LiveCrops.transform.GetChild(i).GetComponent<Crop>();
            crop.OnCycleEnd();
        }
    }

    private void setMusicIntensity(int i) {
        ActionMusic.SetParameter("Intensity", i);
    }

    private IEnumerator spawnBird() {
        GameObject prefab;
        GameObject spawnedBird;
        while (true) {
            if (GameState == GameStates.Scarecrow) {
                //prefab = birdSpawner.Choose();
                prefab = CrowPrefab;
                spawnedBird = Instantiate(prefab, Birds);
                Vector3 spawn = Player.transform.position + UnityEngine.Random.onUnitSphere * prefab.GetComponent<Bird>().SpawnDistance;
                if (spawn.y < 0f) {
                    spawn.y = -spawn.y;
                }
                if (spawn.y < 10f) {
                    spawn.y = 10f;
                }
                spawnedBird.transform.position = spawn;
                if (Birds.childCount > IntensityThreshold2) {
                    setMusicIntensity(2);
                } else if (Birds.childCount > IntensityThreshold1) {
                    setMusicIntensity(1);
                } else {
                    setMusicIntensity(0);
                }
                yield return new WaitForSeconds(spawnedBird.GetComponent<Bird>().SpawnInterval * SpawnIntervalMultiplier);
            } else {
                yield return null;
            }
        }
    }

}

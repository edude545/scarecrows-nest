using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    public GameObject CrowPrefab;
    public GameObject BasicCropPrefab;

    public float WaggleScoreMultiplier = 0.25f;
    public float SpawnInterval = 3f;

    public float SpawnDistance = 100f;

    public static GameObject Player;
    public static GameObject LeftHand;
    public static GameObject RightHand;
    public static GameObject Head;

    public HashSet<Crop> Crops = new HashSet<Crop>(); 
    public HashSet<GameObject> SeedBags = new HashSet<GameObject>();
    public Crow[] Birds;

    public Dictionary<string, int> Resources = new Dictionary<string, int>();

    GameState gameState = GameState.Farm;
    float roundTimer = 0f; // seconds
    float roundEndTime = 60f;

    public static float LeftArmExtension;
    public static float RightArmExtension;
    public static Vector3 LeftHandLastPos;
    public static Vector3 RightHandLastPos;
    public static float LeftHandPosDelta;
    public static float RightHandPosDelta;
    public static float WaggleScore;

    public bool KBMDebug = false;
    public GameObject KBMPlayerPrefab;
    public GameObject VRPlayerPrefab;
    public Canvas UICanvasPrefab;

    enum GameState {
        Farm, // Allow player to plant seeds
        Shop, // Allow player to buy upgrades
        Scarecrow, // Spawn birds
        ScarecrowEnd, // No birds spawn; move to next game phase once all birds have been scared away
    }

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        Player = Instantiate(KBMDebug ? KBMPlayerPrefab : VRPlayerPrefab);
        Canvas canv = Instantiate(UICanvasPrefab);
        if (!KBMDebug)
        {
            Transform steamVRObjects = Player.transform.Find("SteamVRObjects");
            LeftHand = steamVRObjects.Find("LeftHand").gameObject;
            RightHand = steamVRObjects.Find("RightHand").gameObject;
            Head = steamVRObjects.Find("VRCamera").gameObject;
            canv.worldCamera = Head.GetComponent<Camera>();
        } else {
            canv.worldCamera = Player.transform.GetChild(0).GetComponent<Camera>();
        }
        canv.planeDistance = 0.5f;
    }

    private void Update()
    {
        if (gameState == GameState.Scarecrow) {
            calculateWaggle();
            roundTimer += Time.deltaTime;
            if (roundTimer >= roundEndTime || Input.GetKeyDown("c")) {
                changeGameState(GameState.ScarecrowEnd);
            }
        } else if (gameState == GameState.ScarecrowEnd) {
            calculateWaggle();
            if (Birds.Length == 0) {
                changeGameState(GameState.Farm);
            }
        } else if (gameState == GameState.Farm) {
            if (Input.GetKeyDown("m")) {
                changeGameState(GameState.Shop);
            }
        } else if (gameState == GameState.Shop) {
            if (Input.GetKeyDown("m")) {
                changeGameState(GameState.Farm);
            }
        }
    }

    private void changeGameState(GameState gs) {
        gameState = gs;
        if (gameState == GameState.Scarecrow) {
            StartCoroutine(spawnBird());
        } else if (gameState == GameState.ScarecrowEnd) {
        } else if (gameState == GameState.Farm) {
        } else if (gameState == GameState.Shop) {
        }
    }

    private void calculateWaggle() {
        if (!KBMDebug) {
            LeftHandPosDelta = (LeftHandLastPos - LeftHand.transform.position).magnitude;
            RightHandPosDelta = (RightHandLastPos - RightHand.transform.position).magnitude;
            LeftHandLastPos = LeftHand.transform.position;
            RightHandLastPos = RightHand.transform.position;
            LeftArmExtension = (Head.transform.position - LeftHand.transform.position).magnitude;
            RightArmExtension = (Head.transform.position - RightHand.transform.position).magnitude;
            WaggleScore = Mathf.Clamp01(LeftHandPosDelta + RightHandPosDelta) * WaggleScoreMultiplier;
        }
    }

    private void onCycleEnd() {
        foreach (Crop crop in Crops)
        {
            int yield = crop.OnCycleEnd();
            if (yield > 0)
            {
                int n = Resources.ContainsKey(crop.PlantType.Name) ? Resources[crop.PlantType.Name] : 0;
                Resources[crop.PlantType.Name] = n + yield;
            }
        }
    }

    private IEnumerator spawnBird() {
        GameObject bird;
        while (true) {
            bird = Instantiate(CrowPrefab);
            Vector3 spawn = Random.onUnitSphere * SpawnDistance;
            if (spawn.y < 0) {
                spawn.y = -spawn.y;
            }
            if (spawn.y < 10) {
                spawn.y = 10;
            }
            bird.transform.position = spawn;
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

}

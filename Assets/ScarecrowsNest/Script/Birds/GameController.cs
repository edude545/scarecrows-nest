using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameController : MonoBehaviour
{
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

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        //StartCoroutine(spawnBird());
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
        if (!KBMDebug)
        {
            LeftHandPosDelta = (LeftHandLastPos - LeftHand.transform.position).magnitude;
            RightHandPosDelta = (RightHandLastPos - RightHand.transform.position).magnitude;
            LeftHandLastPos = LeftHand.transform.position;
            RightHandLastPos = RightHand.transform.position;
            LeftArmExtension = (Head.transform.position - LeftHand.transform.position).magnitude;
            RightArmExtension = (Head.transform.position - RightHand.transform.position).magnitude;
            WaggleScore = Mathf.Clamp01(LeftHandPosDelta + RightHandPosDelta) * WaggleScoreMultiplier;
        }
    }

    void onCycleEnd() {

    }

    private IEnumerator spawnBird()
    {
        GameObject bird;
        while (true)
        {
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

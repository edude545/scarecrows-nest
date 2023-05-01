using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class ScarecrowPlayer : MonoBehaviour
{

    public bool FarmerMode = true;

    BeltObject heldObject;

    public SteamVR_Action_Boolean DebugModeToggle;

    public GameObject LeftHandModelPrefabScarecrow;
    public GameObject RightHandModelPrefabScarecrow;
    public GameObject LeftHandModelPrefabFarmer;
    public GameObject RightHandModelPrefabFarmer;

    protected Crop SelectedCrop;

    public float DebugSpookAmount = 0.2f;

    private void Start()
    {
        /*SteamVR_LaserPointer laser = RightHand.GetComponent<SteamVR_LaserPointer>();
        laser.PointerIn += PointerInside;
        laser.PointerOut += PointerOutside;
        laser.PointerClick += PointerClick;*/
        DebugModeToggle.AddOnStateUpListener(debugToggleMode, SteamVR_Input_Sources.RightHand);
    }

    private void Update() {
        if (Input.GetKeyDown("space")) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bird")) {
                obj.GetComponent<Crow>().Spook(DebugSpookAmount);
            }
        }
    }

    /*public void PointerClick(object sender, PointerEventArgs ev)
    {
        Crop crop = ev.target.gameObject.GetComponent<Crop>();
        if (crop != null)
        {
            SelectedCrop = crop;
        }
    }

    public void PointerInside(object sender, PointerEventArgs ev)
    {
        Crop crop = ev.target.gameObject.GetComponent<Crop>();
        if (crop != null)
        {
            crop.OnPointerEnter();
        }
    }

    public void PointerOutside(object sender, PointerEventArgs ev)
    {
        Crop crop = ev.target.gameObject.GetComponent<Crop>();
        if (crop != null)
        {
            crop.OnPointerExit();
        }
    }*/

    protected void debugToggleMode(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        if (FarmerMode)
        {
            FarmerMode = false;
            //RightHand.GetComponent<SteamVR_LaserPointer>().active = false;
            if (!GameController.Instance.VRFallback) {
                GameController.Instance.LeftHand.GetComponent<Hand>().SetRenderModel(LeftHandModelPrefabScarecrow);
                GameController.Instance.RightHand.GetComponent<Hand>().SetRenderModel(RightHandModelPrefabScarecrow);
            }
        } else
        {
            FarmerMode = true;
            //RightHand.GetComponent<SteamVR_LaserPointer>().active = true;
            if (!GameController.Instance.VRFallback) {
                GameController.Instance.LeftHand.GetComponent<Hand>().SetRenderModel(LeftHandModelPrefabFarmer);
                GameController.Instance.RightHand.GetComponent<Hand>().SetRenderModel(RightHandModelPrefabFarmer);
            }
        }

        foreach (GameObject obj in GameController.Instance.SeedBags)
        {
            obj.SetActive(true);
        }

        Debug.Log("Farmer mode: " + FarmerMode);
    }


}

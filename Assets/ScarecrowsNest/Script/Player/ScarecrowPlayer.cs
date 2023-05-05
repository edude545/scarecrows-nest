using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class ScarecrowPlayer : MonoBehaviour
{

    BeltObject heldObject;

    public SteamVR_Action_Boolean DebugModeToggle;

    protected Crop SelectedCrop;

    private void Start()
    {
        /*SteamVR_LaserPointer laser = RightHand.GetComponent<SteamVR_LaserPointer>();
        laser.PointerIn += PointerInside;
        laser.PointerOut += PointerOutside;
        laser.PointerClick += PointerClick;*/
        DebugModeToggle.AddOnStateUpListener(GameController.Instance.DebugToggleMode, SteamVR_Input_Sources.RightHand);
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

}

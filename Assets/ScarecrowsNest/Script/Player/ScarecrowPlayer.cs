using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class ScarecrowPlayer : MonoBehaviour
{

    public bool FarmerMode;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Head;

    BeltObject heldObject;

    public SteamVR_Action_Boolean LaserPointer;
    public SteamVR_Action_Boolean DebugModeToggle;

    private void Start()
    {
        LaserPointer.AddOnStateDownListener(onLaserPointerOn, SteamVR_Input_Sources.Any);
        LaserPointer.AddOnStateUpListener(onLaserPointerOff, SteamVR_Input_Sources.Any);
        DebugModeToggle.AddOnStateUpListener(debugToggleMode, SteamVR_Input_Sources.Any);
    }

    protected void onLaserPointerOn(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        if (FarmerMode)
        {
            Debug.Log(source.ToString()); // This prints "any" :(
            if (source == SteamVR_Input_Sources.RightHand) {
                LeftHand.GetComponent<SteamVR_LaserPointer>().active = true;
            } else if (source == SteamVR_Input_Sources.LeftHand) {
                RightHand.GetComponent<SteamVR_LaserPointer>().active = true;
            }
        }
    }

    protected void onLaserPointerOff(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        if (FarmerMode)
        {
            if (source == SteamVR_Input_Sources.RightHand)
            {
                LeftHand.GetComponent<SteamVR_LaserPointer>().active = false;
            }
            else if (source == SteamVR_Input_Sources.LeftHand)
            {
                RightHand.GetComponent<SteamVR_LaserPointer>().active = false;
            }
        }
    }

    protected void debugToggleMode(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        if (FarmerMode)
        {
            FarmerMode = false;
            LeftHand.GetComponent<SteamVR_LaserPointer>().active = false;
            RightHand.GetComponent<SteamVR_LaserPointer>().active = false;
        } else
        {
            FarmerMode = true;
        }
        Debug.Log(FarmerMode);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class Seed : MonoBehaviour
{

    public Plant PlantType;
    public void OnAttach()
    {
        Debug.Log("Seed attached successfully");
    }

    public void OnDetach()
    {
        Debug.Log("Seed detached!");
    }

}

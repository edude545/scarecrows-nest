using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropInitializer : MonoBehaviour
{

    public Plant PlantType;

    void Start()
    {
        Crop crop = GetComponent<Crop>();
        for (int i = 0; i < PlantType.RequiredSeeds; i++) {
            crop.ReceiveSeed(PlantType);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{

    public Crop Crop;

    void Start()
    {
        Crop = transform.parent.GetComponent<Crop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Seed seed = other.GetComponent<Seed>();
        if (seed != null)
        {
            Crop.ReceiveSeed(seed.PlantType);
            seed.ResetSeed();
        }
    }

}

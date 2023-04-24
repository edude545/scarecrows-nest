using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{

    Crop crop;

    void Start()
    {
        crop = transform.parent.GetComponent<Crop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Seed seed = other.GetComponent<Seed>();
        if (seed != null)
        {
            crop.ReceiveSeed(seed);
            Destroy(seed.gameObject);
        }
    }

}

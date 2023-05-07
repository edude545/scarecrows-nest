using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SeedBag : UIElement
{

    public Plant PlantType;
    public GameObject SeedPrefab;
    public void SpawnSeed(Hand hand)
    {
        if (hand == null)
        {
            Debug.Log("Hand is null");
        } else
        {
            GameObject seed = Instantiate(SeedPrefab);
            hand.AttachObject(seed, GrabTypes.Trigger);
        }
    }

    protected override void OnButtonClick()
    {
        SpawnSeed(currentHand);
        onHandClick.Invoke(currentHand);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BeltObject : Throwable
{

    public ScarecrowPlayer Player;

    protected Vector3 beltSnapPos;
    protected Quaternion beltSnapRot;

    public void Awake()
    {
        beltSnapPos = transform.position;
        beltSnapRot = transform.rotation;
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        attachRotation = Quaternion.identity;
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);
        transform.position = beltSnapPos;
        transform.rotation = beltSnapRot;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BeltObject : Throwable
{

    public ScarecrowPlayer Player;

    protected Vector3 beltSnapPos;
    protected Quaternion beltSnapRot;

    protected override void Awake()
    {
        base.Awake();
        beltSnapPos = transform.position;
        beltSnapRot = transform.rotation;
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        transform.localRotation = Quaternion.identity;
        attachRotation = Quaternion.identity;
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        attached = false;
        onDetachFromHand.Invoke();
        hand.HoverUnlock(null);
        transform.position = beltSnapPos;
        transform.rotation = beltSnapRot;
    }

}

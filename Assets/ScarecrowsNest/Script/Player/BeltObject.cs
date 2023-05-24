using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BeltObject : Throwable
{

    protected Vector3 beltSnapPos;
    protected Quaternion beltSnapRot;

    protected override void Awake()
    {
        base.Awake();
        beltSnapPos = transform.localPosition;
        beltSnapRot = transform.localRotation;
    }

    public virtual void Use(float triggerValue) {
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        GameController.Instance.OnBeltObjectAttached(this, hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        // Original code
        attached = false;
        onDetachFromHand.Invoke();
        hand.HoverUnlock(null);
        //

        transform.localPosition = beltSnapPos;
        transform.localRotation = beltSnapRot;
        GameController.Instance.OnBeltObjectDetached(hand);
    }

}

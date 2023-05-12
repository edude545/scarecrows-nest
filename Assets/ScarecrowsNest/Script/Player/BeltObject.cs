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
        beltSnapPos = transform.position;
        beltSnapRot = transform.rotation;
    }

    public virtual void Use(float triggerValue) {
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        transform.localRotation = Quaternion.identity;
        attachRotation = Quaternion.identity;
        GameController.Instance.OnBeltObjectAttached(this, hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        // Original code
        attached = false;
        onDetachFromHand.Invoke();
        hand.HoverUnlock(null);
        //

        transform.position = beltSnapPos;
        transform.rotation = beltSnapRot;
        GameController.Instance.OnBeltObjectDetached(hand);
    }

}

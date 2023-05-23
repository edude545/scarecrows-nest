using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class UpgradeItem : MonoBehaviour
{
    public enum Upgrades {
        UnlockSeedPumpkin,
        UnlockSeedTomato,
        UnlockSeedPepper,
        
        ToolNoiseGun,
        ToolPepperSpray,

        PumpkinHead,        // Multiply spook factor when looking at birds. Also absorbs damage, but can be broken.
        PepperSprayRefill
    }

    public Upgrades UpgradeType;
    public Dictionary<Plant, int> Cost;
    public bool ResetAfterBuying = true;

    private Transform shelf;
    private Vector3 originalPosition;
    private Rigidbody rb;

    private void Awake() {
        shelf = transform.parent;
        originalPosition = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update() {
        if (transform.position.y < -10) {
            ResetItem();
        }
    }

    public void ResetItem() {
        transform.parent = shelf.transform;
        transform.localPosition = originalPosition;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnDetach() {
        rb.useGravity = true;
    }
}

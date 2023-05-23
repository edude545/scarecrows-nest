using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class ShopItem : MonoBehaviour {

    public bool IsSingleUseUpgrade = false;

    public Dictionary<Plant, int> Cost;

    private Transform shelf;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public enum Types {
        UnlockPlantPumpkin,
        UnlockPlantPepper,
        
        UnlockToolNoiseGun,
        UnlockToolPepperSpray,

        PumpkinHead,
        CapsaicinRefill
    }

    public Types Type;

    private void Awake() {
        shelf = transform.parent;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void ResetTransform() {
        if (IsSingleUseUpgrade) {
            Destroy(this);
        } else {
            transform.parent = shelf;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }

    public void OnPickup() {
        GameController.Instance.ShopWhiteboard.LoadTextFromItem(this);
    }

    public void OnDetach() {
        GameController.Instance.ShopWhiteboard.ClearText();
    }

    public void ApplyUpgrade() {

    }

}

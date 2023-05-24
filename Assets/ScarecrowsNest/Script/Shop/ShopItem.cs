using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class ShopItem : MonoBehaviour
{

    public bool IsSingleUseUpgrade = false;

    public Dictionary<Plant, int> Cost;

    public Plant[] CostKeys;
    public int[] CostValues;

    private Transform shelf;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    public enum Types
    {
        UnlockPlantPumpkin,
        UnlockPlantPepper,

        UnlockToolNoiseGun,
        UnlockToolPepperSpray,

        HayArmor,
        PumpkinHead,
        CapsaicinRefill
    }

    public Types Type;

    private void OnValidate() {
        if (CostKeys.Length == CostValues.Length) {
            Cost = new Dictionary<Plant, int>();
            for (int i = 0; i < CostKeys.Length; i++) {
                Cost[CostKeys[i]] = CostValues[i];
            }
        }
    }

    private void Awake()
    {
        shelf = transform.parent;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        rb = GetComponent<Rigidbody>();
        ResetTransform();
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            ResetTransform();
        }
    }

    public void ResetTransform()
    {
        transform.parent = shelf;
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void OnPickup()
    {
        GameController.Instance.ShopWhiteboard.LoadTextFromItem(this);
        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnDetach()
    {
        GameController.Instance.ShopWhiteboard.ClearText();
        rb.useGravity = true;
    }

    public void ApplyUpgrade()
    {
        if (Type == Types.UnlockPlantPumpkin)
        {
            GameController.Instance.SeedBagPumpkin.SetActive(true);
        }
        else if (Type == Types.UnlockPlantPepper)
        {
            GameController.Instance.SeedBagPepper.SetActive(true);
        }
        else if (Type == Types.UnlockToolNoiseGun)
        {
            GameController.Instance.NoiseGun.gameObject.SetActive(true);
        }
        else if (Type == Types.UnlockToolPepperSpray)
        {
            GameController.Instance.PepperSpray.gameObject.SetActive(true);
            GameController.Instance.PepperSpray.Refill();
        }
        else if (Type == Types.HayArmor)
        {
            GameController.Instance.ChangeBodySize(3f);
        }
        else if (Type == Types.PumpkinHead)
        {
            GameController.Instance.PumpkinArmor = 100f;
        }
        else if (Type == Types.CapsaicinRefill)
        {
            GameController.Instance.PepperSpray.Refill();
        }
    }

}

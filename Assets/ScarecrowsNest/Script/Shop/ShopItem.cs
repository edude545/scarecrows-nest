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

        PumpkinHead,
        CapsaicinRefill
    }

    public Types Type;

    private void Awake()
    {
        shelf = transform.parent;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Awake");
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            ResetTransform();
        }
    }

    public void ResetTransform()
    {
        Debug.Log("Resetting transform");
        if (IsSingleUseUpgrade)
        {
            Destroy(this);
        }
        else
        {
            transform.parent = shelf;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnPickup()
    {
        Debug.Log("Picked up");
        GameController.Instance.ShopWhiteboard.LoadTextFromItem(this);
    }

    public void OnDetach()
    {
        Debug.Log("Deteached");
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
        else if (Type == Types.CapsaicinRefill)
        {
            GameController.Instance.PepperSpray.Refill();
        }
        else if (Type == Types.PumpkinHead)
        {
            GameController.Instance.PumpkinArmor = 100f;
        }
    }

}

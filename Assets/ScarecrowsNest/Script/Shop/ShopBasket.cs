using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShopBasket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        ShopItem si = other.GetComponent<ShopItem>();
        if (si != null) {
            if (GameController.Instance.AttemptPurchase(si.Cost)) {
                si.ApplyUpgrade();
                GameController.Instance.CheckoutText("Payment Received :)");
                if (si.IsSingleUseUpgrade)
                {
                    Destroy(si.gameObject);
                } else
                {
                    si.ResetTransform();
                }
            } else
            {
                GameController.Instance.CheckoutText("You don't have enough for that, dummy!");
                si.ResetTransform();
            }
        }
    }
}

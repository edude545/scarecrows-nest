using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShopBasket : MonoBehaviour
{
    public Whiteboard Whiteboard;

    private void OnTriggerEnter(Collider other) {
        ShopItem si = other.GetComponent<ShopItem>();
        if (si != null) {
            si.ResetTransform();
            if (GameController.Instance.AttemptPurchase(si.Cost)) {
                si.ApplyUpgrade();
            }
        }
    }

}

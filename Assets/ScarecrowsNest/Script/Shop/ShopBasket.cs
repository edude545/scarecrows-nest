using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShopBasket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        UpgradeItem item = other.GetComponent<UpgradeItem>();
        if (item != null) {
            GameController.Instance.ApplyUpgrade(item.UpgradeType);
            item.ResetItem();
            if (!item.ResetAfterBuying) {
                item.gameObject.SetActive(false);
            }
        }
    }
}

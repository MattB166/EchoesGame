using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text quantityText;

    private void Start()
    {
        itemIcon = GetComponent<Image>();
        quantityText = GetComponentInChildren<TMP_Text>();
    }
    public void UpdateSlot(InventoryItem item)
    {
        //Debug.Log("Updating Inventory Slot");
        if (item != null && item.item != null)
        {
            Debug.Log("Updating Inventory Slot");
            itemIcon.sprite = item.item.itemData.itemSprite;
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
            //gameObject.SetActive(true);
        }
        else
        {
            itemIcon.sprite = null;
            quantityText.text = "";
            //gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Script which manages the player's inventory, including the items they have and the quantity of each item, and using these items. 
/// </summary>
[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;
    public InventoryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
public class Inventory : MonoBehaviour   //////MAYBE CREATE AN INVENTORY SLOT SCRIPT, FOR THE SPRITE AND THE QUANTITY AND CURRENT SELECTED ITEM. 
{
    public List<InventoryItem> items = new List<InventoryItem>();
    private int currentItemIndex = 0;
    public delegate void ItemChanged(InventoryItem item);
    public ItemChanged itemChangedCallback;
    public delegate void ItemUsed(InventoryItem item);
    public ItemUsed itemUsedCallback;
    public GameObject player;
    public InventoryItem currentItem
    {
        get
        {
            if (items.Count == 0)
            {
                return null;
            }
            return items[currentItemIndex];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCycleInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CycleInventory();
        }
    }

    public void CycleInventory()
    {
        if(items.Count == 0)
        {
            return;
        }
        currentItemIndex = (currentItemIndex + 1) % items.Count;
        itemChangedCallback?.Invoke(currentItem);
        //Debug.Log("Current item: " + currentItem.item.itemData.name);

    }

    public void AddItem(Item newItem)
    {
        //Debug.Log("Adding item to inventory: " + newItem.itemData.name);
        //check if the item is already in the inventory
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item.itemData.name == newItem.itemData.name)
            {
                inventoryItem.quantity++;
                //Debug.Log("Item already in inventory, increasing quantity to " + inventoryItem.quantity);
                return;
            }
        }
        //if the item is not in the inventory, add it
        //Debug.Log("Item not in inventory, adding it");
        items.Add(new InventoryItem(newItem, 1));

    }

    public void RemoveItem()
    {
        if (items.Count == 0)
        {
            return;
        }
        items.RemoveAt(currentItemIndex);
        if(items.Count == 0)
        {
            currentItemIndex = 0;
        }
        else
        {
            currentItemIndex = Mathf.Clamp(currentItemIndex, 0, items.Count - 1);
        }
        if(items.Count > 0)
        {
            itemChangedCallback?.Invoke(currentItem);
        }
        
        CycleInventory();
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && items.Count > 0)
        {
            if(currentItem.item != null)
            currentItem.item.Use();
            itemUsedCallback?.Invoke(currentItem);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public InventoryItem currentItem
    {
        get
        {
            return items[currentItemIndex];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CycleInventory()
    {

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
      
    }
}

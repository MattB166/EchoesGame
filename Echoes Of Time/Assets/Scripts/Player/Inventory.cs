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
    private List<InventoryItem> itemsToRemove = new List<InventoryItem>();
    public int currentItemIndex = 0;
    public delegate void ItemChanged(InventoryItem item);
    public ItemChanged itemChangedCallback;
    public delegate void ItemUsed(InventoryItem item);
    public ItemUsed itemUsedCallback;
    public delegate void ItemSecondaryUsed(InventoryItem item);
    public ItemSecondaryUsed itemSecondaryUsedCallback;
    public delegate void ItemDropped(InventoryItem item);
    public ItemDropped itemDroppedCallback;
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


    public Dictionary<ProjectileData, int> storedProjectiles = new Dictionary<ProjectileData, int>();


    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessItemRemovals();
        
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
        if (items.Count == 0)
        {
            return;
        }
        currentItemIndex = (currentItemIndex + 1) % items.Count;
        itemChangedCallback?.Invoke(currentItem);
        //Debug.Log("Current item: " + currentItem.item.itemData.name);

    }

    public void SetCurrentItem(InventoryItem item)
    {
        if(item != null)
        {
            currentItemIndex = items.IndexOf(item);
            itemChangedCallback?.Invoke(currentItem);
        }
        else
        {
            currentItemIndex = 0;
            itemChangedCallback?.Invoke(null);
        }
        
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
        InventoryItem item = new InventoryItem(newItem, 1);
        items.Add(item);
        SetCurrentItem(item);



    }

    public void DropItem()
    {
        GameObject droppedItem = Instantiate(currentItem.item.prefab, player.transform.position, Quaternion.identity);
        items[currentItemIndex].quantity--;
        if (items[currentItemIndex].quantity == 0)
        {
            itemDroppedCallback?.Invoke(currentItem);
            items.RemoveAt(currentItemIndex);
            CycleInventory();
        }
    }

    public void RemoveItem(Item item)
    {
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.item.itemData.name == item.itemData.name)
            {
                inventoryItem.quantity--;
                if (inventoryItem.quantity == 0)
                {
                    itemDroppedCallback?.Invoke(inventoryItem);
                    itemsToRemove.Add(inventoryItem);
                    CycleInventory();
                }
            }
        }
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && items.Count > 0)
        {
            if (currentItem.item != null)
                currentItem.item.Use();
            itemUsedCallback?.Invoke(currentItem);
            //DropItem();
        }
    }

    public void OnSecondaryUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && items.Count > 0)
        {
            if (currentItem.item != null)
            {
                currentItem.item.SecondaryUse();
                itemSecondaryUsedCallback?.Invoke(currentItem);
            }

        }
    }

    private void ProcessItemRemovals()
    {
        foreach (InventoryItem item in itemsToRemove)
        {
            items.Remove(item);
        }
        itemsToRemove.Clear();

    }

    public void StoreProjectile(ProjectileData projectileData, int amount)
    {
        if(storedProjectiles.ContainsKey(projectileData))
        {
            storedProjectiles[projectileData] += amount;
        }
        else
        {
            storedProjectiles.Add(projectileData, amount);
        }
        Debug.Log("Stored " + amount + " " + projectileData.name + " in inventory");
    }

    public int GetStoredProjectileAmount(ProjectileData projectileData)
    {
        if(storedProjectiles.TryGetValue(projectileData, out int amount))
        {
            storedProjectiles.Remove(projectileData);
            Debug.Log("Retrieved " + amount + " " + projectileData.name + " from inventory");
            return amount; 
        }
        return 0;
    }
}

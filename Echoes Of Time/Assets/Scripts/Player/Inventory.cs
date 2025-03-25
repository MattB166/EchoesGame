using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public static Inventory instance;

    public List<InventoryItem> items = new List<InventoryItem>();
    private List<InventoryItem> itemsToRemove = new List<InventoryItem>();
    public int currentItemIndex = 0;
    public GameEvent itemChanged;
    public GameEvent onItemUsed;
    public GameEvent onItemSecondaryUsed;
    public GameEvent onItemDropped;
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



    private void Awake()
    {
        Debug.Log("Inventory awake");
        if (instance == null)
        {
            Debug.Log("Inventory instance created");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.Log("Inventory instance already exists, destroying this one");
            Destroy(gameObject);
            return;
        }
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Vector3 spawnLocation = SpawnManager.instance.GetSpawnLocation(scene.name);
        player.transform.position = spawnLocation;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessItemRemovals();
        DontDestroyOnLoad(gameObject);
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
        
        itemChanged.Announce(this, currentItem);

    }

    public void SetCurrentItem(InventoryItem item)
    {
        if (item != null)
        {
            currentItemIndex = items.IndexOf(item);
            itemChanged.Announce(this, item);
        }
        else
        {
            currentItemIndex = 0;
            itemChanged.Announce(this, currentItem);
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
            onItemDropped.Announce(this, currentItem);
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
                    onItemDropped.Announce(this, inventoryItem);
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
            onItemUsed.Announce(this, currentItem);
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
                onItemSecondaryUsed.Announce(this, currentItem);
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
        //Debug.Log("Stored " + amount + " " + projectileData.name + " in inventory");
    }

    public int GetStoredProjectileAmount(ProjectileData projectileData)
    {
        if(storedProjectiles.TryGetValue(projectileData, out int amount))
        {
            storedProjectiles.Remove(projectileData);
            //Debug.Log("Retrieved " + amount + " " + projectileData.name + " from inventory");
            return amount; 
        }
        return 0;
    }

    public void PlayItemSound()
    {
       currentItem.item.PlayUseSound();
    }
}

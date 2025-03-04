using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Actions;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    public int currentSaveSlot;
    public Transform playerPos { get { return player.transform; } }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        //load slot 
        SavingSystem.DeleteSaveSlot(currentSaveSlot);
        //LoadGame(currentSaveSlot);

    }

    // Update is called once per frame
    void Update()
    {
        playerPos.position = player.transform.position;
        
    }

    private void OnApplicationQuit()
    {
        //get current save slot
        //save the game into current slot. 
        //SaveGame(currentSaveSlot);
        //Debug.Log("Saved game to save slot " + currentSaveSlot);
    }

    public void LoadGame(int currentSaveSlot)
    {
        //check if the save slot exists
        if(!SavingSystem.SaveSlotExists(currentSaveSlot))
        {
            Debug.LogError("Save slot " + currentSaveSlot + " does not exist.");
            player.GetComponent<Actions>().SetCurrentWeapon(Weapons.None);
            return;
        }
        PlayerSaveData playerSaveData = SavingSystem.LoadPlayerData(currentSaveSlot);
        if(playerSaveData != null )
        {
            //load the player data into their classes. 
            Debug.Log("Loaded player data from save slot " + currentSaveSlot);
            HashSet<Weapons> availableWeapons = new HashSet<Weapons>(playerSaveData.availableWeaponsList);
            player.GetComponent<Actions>().GetAvailableWeapons().Clear();
            player.GetComponent<Actions>().SetAvailableWeapons(availableWeapons);

            foreach(Projectiles p in playerSaveData.availableProjectiles)
            {
                player.GetComponent<Inventory>().StoreProjectile(p.projectile.projectileData, p.ammoCount);
            }
            foreach (InventoryItem item in playerSaveData.inventoryItems)
            {
                //get the item, initialise it and add it to the inventory.
                item.item.Init(item.item.itemData, player.GetComponent<Inventory>(), item.item.prefab);
                Debug.Log("item initialised with " + item.item.itemData.name);
                player.GetComponent<Inventory>().AddItem(item.item);
            }
            player.GetComponent<Inventory>().currentItemIndex = playerSaveData.currentInventoryItemIndex;
            player.GetComponent<Actions>().currentWeapon = playerSaveData.currentWeaponIndex;
        }
        else
        {
            Debug.LogError("Failed to load player data from save slot " + currentSaveSlot);
        }

        GameSaveData gameSaveData = SavingSystem.LoadGameData(currentSaveSlot);
        if(gameSaveData != null)
        {
            //load the game data into the game manager and dispense to relevant sections.  
            Debug.Log("Loaded game data from save slot " + currentSaveSlot);
            CheckPointSystem.instance.SetLastActiveLevel(gameSaveData.levelName);
            CheckPointSystem.instance.achievedCheckPointIDs = new HashSet<int>(gameSaveData.achievedCheckPointIDs);
            CheckPointSystem.instance.achievedCheckPoints = gameSaveData.achievedCheckPoints;
            CheckPointSystem.instance.SetCurrentCheckPoint(gameSaveData.activeCheckPoint);
            CheckPointSystem.instance.activeCheckPoint.checkPointID = gameSaveData.checkPointID;


        }
        else
        {
            Debug.LogError("Failed to load game data from save slot " + currentSaveSlot);
        }
    }


    public void SaveGame(int currentSaveSlot)
    {
        ///player data
        Actions pActions = player.GetComponent<Actions>();
        List<InventoryItem> items = player.GetComponent<Inventory>().items;
        List<Actions.Weapons> availableWeapons = new List<Actions.Weapons>(pActions.GetAvailableWeapons());
        Actions.Weapons currentWeapon = pActions.currentWeapon;
        int currentInventoryItemIndex = player.GetComponent<Inventory>().currentItemIndex;
        //get access to bow item projectiles
        List<Projectiles> projectiles = new List<Projectiles>();
        foreach(InventoryItem item in items)
        {
            if(item.item is BowItem bow)
            {
                foreach (Projectiles p in bow.projectiles)
                {
                    projectiles.Add(p);
                }
            }
        }
        PlayerSaveData playerSaveData = new PlayerSaveData(currentWeapon,availableWeapons, items,currentInventoryItemIndex,projectiles,0);
        SavingSystem.SavePlayerData(playerSaveData, currentSaveSlot);

        ///game data 

        string lastLevelName = SceneManager.GetActiveScene().name;
        List<int> levelList = CheckPointSystem.instance.achievedCheckPointIDs.ToList();
        List<CheckPoint> checkPoints = CheckPointSystem.instance.achievedCheckPoints;
        CheckPoint checkPoint = CheckPointSystem.instance.activeCheckPoint;
        int checkPointID = checkPoint.checkPointID;
        GameSaveData gameSaveData = new GameSaveData(lastLevelName, levelList, checkPoints, checkPoint, checkPointID);
        SavingSystem.SaveGameData(gameSaveData, currentSaveSlot);

    }
}

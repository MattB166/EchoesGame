using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Actions;

public enum SceneType
{
    MainMenu,
    Game,
    PauseMenu,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject player { get; private set; }
    public int currentSaveSlot;
    public Transform playerPos { get { return player.transform; } }

    public SceneType CurrentSceneType { get; private set; }

    private bool hasLoaded = false;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
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
        //SavingSystem.DeleteSaveSlot(currentSaveSlot);
        //if (!hasLoaded)
        //{
        //    LoadGame(currentSaveSlot);
        //    hasLoaded = true;
        //}


    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
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
        if (!SavingSystem.SaveSlotExists(currentSaveSlot))
        {
            Debug.LogError("Save slot " + currentSaveSlot + " does not exist.");
            if(player != null)
                player.GetComponent<Actions>().SetCurrentWeapon(Weapons.None);
            return;
        }
        PlayerSaveData playerSaveData = SavingSystem.LoadPlayerData(currentSaveSlot);
        if (playerSaveData != null)
        {
            //load the player data into their classes. 
            Debug.Log("Loaded player data from save slot " + currentSaveSlot);
            HashSet<Weapons> availableWeapons = new HashSet<Weapons>(playerSaveData.availableWeaponsList);
            player.GetComponent<Actions>().GetAvailableWeapons().Clear();
            player.GetComponent<Actions>().SetAvailableWeapons(availableWeapons);

            foreach (Projectiles p in playerSaveData.availableProjectiles)
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

        //game data loading. 
        GameSaveData gameSaveData = SavingSystem.LoadGameData(currentSaveSlot);
        if (gameSaveData != null)
        {
            if (SceneManager.GetActiveScene().name != gameSaveData.levelName)
            {
                CheckPointSystem.instance.lastActiveLevel = gameSaveData.levelName;
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(CheckPointSystem.instance.lastActiveLevel);
            }
            else
            {
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }


        }
        else
        {
            string firstScene = "TestingLevel";
            if (SceneManager.GetActiveScene().name != firstScene)
            {
                CheckPointSystem.instance.lastActiveLevel = firstScene;
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(CheckPointSystem.instance.lastActiveLevel);
            }
            else
            {
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if(player != null)
        {
            Destroy(player);
        }
        Debug.Log("Instantiating new player");
        player = Instantiate(playerPrefab);
        player.name = "Player";


        GameSaveData gameSaveData = SavingSystem.LoadGameData(currentSaveSlot);
        if (gameSaveData == null) return;
        CheckPointSystem.instance.achievedCheckPointIDs = new HashSet<int>(gameSaveData.achievedCheckPointIDs);
        Movement playerMovement = player.GetComponent<Movement>();
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject checkpoint in checkpoints)
        {
            CheckPoint cp = checkpoint.GetComponent<CheckPoint>();
            if (gameSaveData.achievedCheckPointIDs.Contains(cp.checkPointID))
            {
                cp.ActivateCheckPointByTimer();
            }
            if (gameSaveData.checkPointID == cp.checkPointID)
            {
                cp.DoNotCorrectPosition();
                CheckPointSystem.instance.activeCheckPoint = cp;
                playerMovement.ResetPlayerPosition();
                Debug.Log("Player spawned at checkpoint " + cp.gameObject.transform.position);
                break;
            }
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
        foreach (InventoryItem item in items)
        {
            if (item.item is BowItem bow)
            {
                foreach (Projectiles p in bow.projectiles)
                {
                    projectiles.Add(p);
                }
            }
        }
        PlayerSaveData playerSaveData = new PlayerSaveData(currentWeapon, availableWeapons, items, currentInventoryItemIndex, projectiles, 0);
        SavingSystem.SavePlayerData(playerSaveData, currentSaveSlot);

        ///game data 



    }

    public void SetCurrentSceneType(SceneType sceneType)
    {
        CurrentSceneType = sceneType;
        Debug.Log("Current scene type set to " + sceneType);
    }
}

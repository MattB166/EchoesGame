using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Actions;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
        Debug.Log("Saved game to save slot " + currentSaveSlot);
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
            //player.GetComponent<Actions>().currentWeapon = playerSaveData.currentWeaponIndex;
            switch(playerSaveData.currentWeaponIndex)
            {
                case Weapons.Spear:
                    player.GetComponent<Actions>().SetCurrentWeapon(Weapons.Spear);
                    break;
                case Weapons.Sword:
                    player.GetComponent<Actions>().SetCurrentWeapon(Weapons.Sword);
                    break;
                case Weapons.Bow:
                    player.GetComponent<Actions>().SetCurrentWeapon(Weapons.Bow);
                    break;
                case Weapons.None:
                    player.GetComponent<Actions>().SetCurrentWeapon(Weapons.None);
                    break;
            }
            player.GetComponent<Inventory>().items = playerSaveData.inventoryItems;
            player.GetComponent<Inventory>().currentItemIndex = playerSaveData.currentInventoryItemIndex;

            return;


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
        PlayerSaveData playerSaveData = new PlayerSaveData(currentWeapon,availableWeapons, items,currentInventoryItemIndex);
        SavingSystem.SavePlayerData(playerSaveData, currentSaveSlot);

        ///game data 


    }
}

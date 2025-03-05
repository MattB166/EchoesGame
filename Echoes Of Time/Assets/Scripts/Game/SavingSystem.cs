using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavingSystem
{
    //player save data
    //game save data - checkpoints, general stats like enemies killed, time played, etc. 
    #region Player Save Data
    public static string playerDataPath = Application.persistentDataPath + "/playerData.json";
    
    private static string getPlayerDataPath(int slot)
    {
        return Application.persistentDataPath + "/playerData" + slot + ".json";
    }


    public static void SavePlayerData(PlayerSaveData playerSaveData, int slot)
    {
        string path = getPlayerDataPath(slot);
        string json = JsonUtility.ToJson(playerSaveData);
        File.WriteAllText(path, json);
        Debug.Log("Saved player data to " + playerDataPath);
    }

    public static PlayerSaveData LoadPlayerData(int slot)
    {
        string path = getPlayerDataPath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerSaveData playerSaveData = JsonUtility.FromJson<PlayerSaveData>(json);
            Debug.Log("Loaded player data from " + path);
            return playerSaveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion


    #region game save data
    public static string gamedataPath = Application.persistentDataPath + "/gameData.json";

    private static string getGameDataPath(int slot)
    {
        return Application.persistentDataPath + "/gameData" + slot + ".json";
    }

    public static void SaveGameData(GameSaveData gameSaveData, int slot)
    {
        string path = getGameDataPath(slot);
        string json = JsonUtility.ToJson(gameSaveData);
        File.WriteAllText(path, json);
        Debug.Log("Saved game data to " + path);
    }

    public static GameSaveData LoadGameData(int slot)
    {
        string path = getGameDataPath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log("Loaded game data from " + path);
            return gameSaveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    #endregion


    #region Save Slot Management
    public static void DeleteSaveSlot(int slot)
    {
        if(File.Exists(getPlayerDataPath(slot)))
        {
            File.Delete(getPlayerDataPath(slot));
            Debug.Log("Deleted save slot " + slot);
        }
        else
        {
            Debug.LogError("Save slot " + slot + " does not exist.");
        }

        if(File.Exists(getGameDataPath(slot)))
        {
            File.Delete(getGameDataPath(slot));
            Debug.Log("Deleted game data in save slot " + slot);
        }
        else
        {
            Debug.LogError("Game data in save slot " + slot + " does not exist.");
        }

    }

    //check if save slot exists
    public static bool SaveSlotExists(int slot)
    {
        return File.Exists(getPlayerDataPath(slot));
    }
    #endregion
}

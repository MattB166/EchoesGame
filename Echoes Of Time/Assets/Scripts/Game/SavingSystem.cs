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
    }

    //check if save slot exists
    public static bool SaveSlotExists(int slot)
    {
        return File.Exists(getPlayerDataPath(slot));
    }

}

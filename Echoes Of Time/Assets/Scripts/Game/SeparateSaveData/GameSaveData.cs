using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    //checkpoint and level data
    public string levelName;
    public List<int> achievedCheckPointIDs;
    public int checkPointID;
    public Vector2 spawnPos;

    public GameSaveData(string levelName, List<int> achievedCheckPointIDs, int checkPointID, Vector2 respawnPos)
    {
        this.levelName = levelName;
        this.achievedCheckPointIDs = achievedCheckPointIDs;
        this.checkPointID = checkPointID;
        this.spawnPos = respawnPos;
    }
}

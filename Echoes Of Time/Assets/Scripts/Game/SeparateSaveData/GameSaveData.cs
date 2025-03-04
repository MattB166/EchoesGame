using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    //checkpoint and level data
    public string levelName;
    public List<int> achievedCheckPointIDs;
    public List<CheckPoint> achievedCheckPoints;
    public CheckPoint activeCheckPoint;
    public int checkPointID;

    public GameSaveData(string levelName, List<int> achievedCheckPointIDs, List<CheckPoint> achievedCheckPoints, CheckPoint activeCheckPoint, int checkPointID)
    {
        this.levelName = levelName;
        this.achievedCheckPointIDs = achievedCheckPointIDs;
        this.achievedCheckPoints = achievedCheckPoints;
        this.activeCheckPoint = activeCheckPoint;
        this.checkPointID = checkPointID;
    }
}

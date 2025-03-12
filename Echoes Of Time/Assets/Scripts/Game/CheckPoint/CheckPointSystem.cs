using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages most recent checkpoint, spawning player at that checkpoint. 
/// </summary>
public class CheckPointSystem : MonoBehaviour
{

    public static CheckPointSystem instance { get; private set; }
    //list of achieved checkpoints
    public List<CheckPoint> achievedCheckPoints = new List<CheckPoint>();
    public HashSet<int> achievedCheckPointIDs = new HashSet<int>();
    public CheckPoint activeCheckPoint;
    public string lastActiveLevel;
    public GameEvent OnNewCheckPoint;
    //member of most recent checkpoint

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        //bring in the saved checkpoint data, and the most recent checkpoint data.

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNewCheckpoint(CheckPoint checkpoint)
    {
        if (!achievedCheckPointIDs.Contains(checkpoint.checkPointID))
        {
            achievedCheckPoints.Add(checkpoint);
            achievedCheckPointIDs.Add(checkpoint.checkPointID);
        }
        activeCheckPoint = checkpoint;
        lastActiveLevel = checkpoint.levelName;

        //save data after checkpoint is set.
        if(OnNewCheckPoint != null)
        {
            OnNewCheckPoint.Announce(this, checkpoint);  ///ADD AN EVENT OR THE SYSTEM DOESNT SAVE IT. 
        }

        SavingSystem.SaveGameData(new GameSaveData(lastActiveLevel, new List<int>(achievedCheckPointIDs), activeCheckPoint.checkPointID, activeCheckPoint.gameObject.transform.position),GameManager.instance.currentSaveSlot);
    }
}

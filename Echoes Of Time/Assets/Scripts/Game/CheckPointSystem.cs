using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    public CheckPoint activeCheckPoint { get; private set; }
    public string lastActiveLevel { get; private set; }
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
        LoadCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNewCheckpoint(CheckPoint checkPoint)
    {
        achievedCheckPoints.Add(checkPoint);
        activeCheckPoint = checkPoint;
        Debug.Log("Current Checkpoint set to " + activeCheckPoint.checkPointID);
    }

    public void SetCurrentCheckPoint(CheckPoint checkPoint)
    {
        activeCheckPoint = checkPoint;
        
    }

    public bool CheckPointActivated(int checkPointID)
    {
        return achievedCheckPointIDs.Contains(checkPointID);
    }

    public void LoadCheckPoint()
    {
        if (!string.IsNullOrEmpty(lastActiveLevel))
        {
            SceneManager.LoadScene(lastActiveLevel);
            //load the player at the last checkpoint.
        }

    }

    public void SetLastActiveLevel(string levelName)
    {
        lastActiveLevel = levelName;
    }
}

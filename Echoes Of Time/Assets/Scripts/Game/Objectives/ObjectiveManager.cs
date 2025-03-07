using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance { get; private set; }

    public BaseObjective mainObjective;
    public List<BaseObjective> sideObjectivePool = new List<BaseObjective>();
    public List<BaseObjective> activeSideObjectives = new List<BaseObjective>();
    public List<BaseObjective> completedObjectives = new List<BaseObjective>();
    public List<BaseObjective> completedSideObjectives = new List<BaseObjective>();
    public int maxSideObjectives = 3;
    public GameEvent OnMainObjectiveUpdated;
    public GameEvent OnMainObjectiveComplete;
    public GameEvent OnSideObjectiveUpdated;
    public GameEvent OnSideObjectiveComplete;

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
        if (sideObjectivePool == null || sideObjectivePool.Count == 0)
        {
            Debug.LogError("SideObjectivePool is not set!");
            return;
        }
        PopulateSideObjectives();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMainObjective(BaseObjective objective)
    {
        mainObjective = objective;
        OnMainObjectiveUpdated.Announce(this, mainObjective);
        objective.Activate();
    }

    public void CompleteObjective(BaseObjective objective)
    {
        if (!completedObjectives.Contains(objective))
        {
            completedObjectives.Add(objective);
        }
        if (objective == mainObjective)
        {
            mainObjective = null;
            OnMainObjectiveComplete.Announce(this, objective);
        }
        else
        {
            activeSideObjectives.Remove(objective);
            completedSideObjectives.Add(objective);
            OnSideObjectiveComplete.Announce(this, objective);
        }


    }

    public void AddSideObjective(BaseObjective objective)
    {
        if (activeSideObjectives.Count < maxSideObjectives)
        {
            activeSideObjectives.Add(objective);
            OnSideObjectiveUpdated.Announce(this, objective);
            objective.Activate();

        }
    }

    public void ResetObjectives()
    {

    }

    public void PopulateSideObjectives()
    {
        //choose between 1 and maxSideObjectives objectives from the pool
        //add them to the active side objectives list
        //activate them
        activeSideObjectives.Clear();
        if (sideObjectivePool == null)
        {
            Debug.LogError("sideObjectivePool is null!");
            return;
        }

        if (sideObjectivePool.Count == 0)
        {
            Debug.LogError("sideObjectivePool is empty!");
            return;
        }

        for (int i = 0; i < Random.Range(1, maxSideObjectives); i++)
        {
            BaseObjective objective = sideObjectivePool[Random.Range(0, sideObjectivePool.Count)];
            if (!activeSideObjectives.Contains(objective))
            {
                activeSideObjectives.Add(objective);
                OnSideObjectiveUpdated.Announce(this, objective);
                objective.Activate();
            }
        }
    }
}

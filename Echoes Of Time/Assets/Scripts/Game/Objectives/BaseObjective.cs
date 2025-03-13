using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Objective class for all objectives in the game
/// </summary>
[System.Serializable]
public abstract class BaseObjective : MonoBehaviour
{
    public ObjectiveData objectiveData;
    public bool isStarted { get; protected set; } = false;
    public bool isCompleted { get; protected set; } = false;
    public float currentProgress; //percentage of completion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!isCompleted && CheckObjectiveCompletion())
        {
            CompleteObjective();
        }
    }

    public virtual void Activate()
    {
        isStarted = true;
    }

    protected void CompleteObjective()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            
            ObjectiveManager.instance.CompleteObjective(this);
            gameObject.SetActive(false);
            //Debug.Log("Objective Complete: " + objectiveData.objectiveName);
        }
    }

    public virtual void ResetObjective()
    {
        isCompleted = false;
        //if required, put progress back to 0. 
    }

    protected abstract bool CheckObjectiveCompletion();
}

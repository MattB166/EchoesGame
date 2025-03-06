using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Objective class for all objectives in the game
/// </summary>
public abstract class BaseObjective : MonoBehaviour
{
    public ObjectiveData objectiveData;
    public bool isCompleted { get; protected set; } = false;

    public GameEvent OnObjectiveComplete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (!isCompleted && CheckObjectiveCompletion())
        {
            CompleteObjective();
        }
    }

    protected void CompleteObjective()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            OnObjectiveComplete.Announce(this,objectiveData);

        }
    }

    public virtual void ResetObjective()
    {
        isCompleted = false;
        //if required, put progress back to 0. 
    }

    protected abstract bool CheckObjectiveCompletion();
}

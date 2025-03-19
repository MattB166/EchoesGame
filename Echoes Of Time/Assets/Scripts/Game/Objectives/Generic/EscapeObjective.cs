using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ObjectiveStage
{
    public string stageName;
    public List<GameEvent> eventsNeeded;
    public float progress;
    public GameEvent stageCompletionEvent;


    public void IncreaseProgress()
    {
        progress += 100 / eventsNeeded.Count;
        progress = Mathf.Clamp(progress, 0, 100f);
    }

    public bool isCompleted => progress >= 100f;
}
public class EscapeObjective : BaseObjective
{
    //an objective for the alignment of switches to be completed
    //requires certain events to be triggered, in order to be completed. 
    private Collider2D triggerCollider;
    public List<ObjectiveStage> escapeStages;
    public int currentStage;
    public int completedStages;
    public List<GameEventListener> currentListeners = new List<GameEventListener>();
    public override void Activate()
    {
        base.Activate();
        //Debug.Log("Escape Objective Activated");
        currentStage = 0;
        InitialiseNewStage(escapeStages[currentStage]);
        //set to first stage. for each event in the first stage, add a new listener to the event.
    }

    public void InitialiseNewStage(ObjectiveStage stage)
    {
        Debug.Log("Initialising new stage: " + stage.stageName);
        currentListeners.Clear();
        foreach (GameEvent gameEvent in stage.eventsNeeded)
        {
            GameEventListener listener = transform.AddComponent<GameEventListener>();
            listener.Init(gameEvent, (component, data) => OnEventTriggered(stage));
            currentListeners.Add(listener);
        }
    }

    public void OnEventTriggered(ObjectiveStage stage)
    {
        stage.IncreaseProgress();
        if (stage.isCompleted)
        {
            stage.stageCompletionEvent.Announce(this, null);
            CompleteStage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = gameObject.GetComponent<Collider2D>();
        Debug.Log("Escape Objective found");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the escape zone");
            ObjectiveManager.instance.SetMainObjective(this);
            triggerCollider.enabled = false;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    protected override bool CheckObjectiveCompletion()
    {
        if (currentStage >= escapeStages.Count)
        {
            Debug.Log("Escape Objective Completed");
            return true;
        }
        return false;
    }

    public void CompleteStage()
    {
        if (currentStage >= escapeStages.Count)
        {
            return;
        }

        foreach (GameEventListener listener in currentListeners)
        {
            Destroy(listener);
        }
        currentListeners.Clear();

        //complete the stage, progress to the next stage.
        completedStages++;
        currentStage++;
        currentProgress = (float)completedStages / escapeStages.Count;

        if (currentStage < escapeStages.Count)
        {
            InitialiseNewStage(escapeStages[currentStage]);
        }

    }
}

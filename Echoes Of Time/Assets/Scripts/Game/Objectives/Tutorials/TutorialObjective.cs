using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameEventListener))]
[RequireComponent(typeof(Collider2D))]
public class TutorialObjective : BaseObjective
{
    public bool tutorialComplete = false;
    public bool actionPerformed = false;
    private Collider2D triggerCollider;
    public override void Activate()
    {
        base.Activate();
        //Debug.Log("Tutorial Objective Activated : " + objectiveData.objectiveName);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(tutorialComplete)
        {
            Destroy(gameObject);
        }
        triggerCollider = gameObject.GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //set objective and apply tutorial message
            ObjectiveManager.instance.SetMainObjective(this);
            //gameObject.transform.SetParent(collision.gameObject.transform); not needed for tutorials. 
        }
    }

    public void SetActionComplete()
    {
        if(isStarted)
        {
            actionPerformed = true;
        }
        
    }

    protected override bool CheckObjectiveCompletion()
    {
        if (actionPerformed && isStarted)
        {
            return true;
        }
        return false;
    }
}

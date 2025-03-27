using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelObjective : BaseObjective
{
    public Vector2 beginLocation;
    public Transform targetTransform;
    private Vector2 targetLocation;
    public float distanceToTarget;
    public float completionRadius;
    private bool isTriggered = false;
    private Collider2D triggerCollider;
    private float highestProgress;
    public override void Activate()
    {
        base.Activate();
        Debug.Log("Travel Objective Activated");
        beginLocation = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        targetLocation = new Vector2(targetTransform.position.x, targetTransform.position.y);
        distanceToTarget = Vector2.Distance(beginLocation, targetLocation);
        highestProgress = 0;
        triggerCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            ObjectiveManager.instance.SetMainObjective(this);
            gameObject.transform.SetParent(collision.gameObject.transform);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        distanceToTarget = Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), targetLocation);
        currentProgress = Mathf.Abs(1 - (distanceToTarget / Vector2.Distance(beginLocation, targetLocation)));
        LockProgress();
        base.Update();
    }


    protected override bool CheckObjectiveCompletion()
    {
        if (distanceToTarget <= completionRadius)
        {
            currentProgress = 1;
            gameObject.transform.SetParent(null);
            Destroy(targetTransform.gameObject);
            return true;
        }
        return false;
    }

    private void LockProgress()
    {
        //stop progress from going backwards
        if (currentProgress < highestProgress)
        {
            currentProgress = highestProgress;
        }
        else
        {
            highestProgress = currentProgress;
            //event to update progress
            //ObjectiveManager.instance.OnMainObjectiveUpdated.Announce(this, this);
        }
    }
}

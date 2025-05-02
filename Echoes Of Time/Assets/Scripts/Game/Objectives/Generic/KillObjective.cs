using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjective : BaseObjective
{
    public int targetCount;
    public int currentCount;
    

    public override void Activate()
    {
        base.Activate();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKill()
    {
        currentCount++;
        currentProgress = (float)currentCount / (float)targetCount;
        
    }


    protected override bool CheckObjectiveCompletion()
    {
        return currentCount >= targetCount;
    }
}

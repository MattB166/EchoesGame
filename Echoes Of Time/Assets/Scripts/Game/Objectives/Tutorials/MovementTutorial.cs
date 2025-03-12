using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTutorial : TutorialObjective
{
    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void Update()
    {
        base.Update();
    }
    protected override bool CheckObjectiveCompletion()
    {
        if (actionPerformed)
        {
            tutorialComplete = true;
            return true;
        }
        return false;
    }


}

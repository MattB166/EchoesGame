using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedIdle : BaseGroundedState
{

    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI.currentState = GroundedStates.Idle;
    }
    public override void RunLogic()
    {
        //Debug.Log("Grounded Idle");
        //keep position exactly where it is, applying gravity so just stays on floor. 
        if(aiCharacter != null)
        {
            //Debug.Log("Grounded Idle active with ai character set, velocity set to 0 on x");
            if(rb != null && groundedAI != null)
            {
                Debug.Log("Grounded Idle active with rb and groundedAI set, velocity set to 0 on x");
                rb.velocity = new Vector2(0, rb.velocity.y);
                
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedIdle : BaseGroundedState
{

    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI.currentState = GroundedStates.Idle;
        aiCharacter.aiPath.maxSpeed = aiCharacter.cachedSpeed * aiCharacter.CustomTimeScale;
    }
    public override void RunLogic()
    {
        if (aiCharacter.CustomTimeScale == 0)
        {
            rb.velocity = Vector2.zero; // Freeze the AI
            return;
        }
        //Debug.Log("Grounded Idle");
        //keep position exactly where it is, applying gravity so just stays on floor. 
        if (aiCharacter != null)
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


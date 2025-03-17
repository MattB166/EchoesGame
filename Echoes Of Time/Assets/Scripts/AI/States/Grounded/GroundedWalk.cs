using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedWalk : BaseGroundedState
{

    private bool targetSet = false;
    private Vector2 target;
    private float walkDistance;
    private bool shouldFlip = false;
    private float walkTimer = 0f;
    public override void OnEnable()
    {
        base.OnEnable();
        aiCharacter.aiPath.enableRotation = false;
        anim.Play(gameObject.name + "_Walk");
        CalculateTarget();
    }
    public override void RunLogic()
    {
        //move character in direction of facing direction for walk time, then change to idle.
        if (targetSet)
        {
            WalkToTarget();
        }
        else
        {
            CalculateTarget();
        }
    }

    public void CalculateTarget()
    {
        Debug.Log("Calculating target");
        Vector2 startPos = transform.position;
        float direction = 0f;
        float distance = 0f;
        if (aiCharacter != null)
        {
            direction = aiCharacter.direction;
            distance = aiCharacter.AICharacterData.patrolDistance;
        }
        else
        {
            Debug.LogError("AICharacter is null in GroundedWalk");
        }
       
        if(!targetSet)
        {
            //calculate target based on direction and distance
            target = new Vector2(startPos.x + (direction * distance), startPos.y);
            targetSet = true;
            Debug.Log("Target set to " + target);
            aiCharacter.direction = -direction;
        }

    }

    public void WalkToTarget()
    {
        //move across to target
        if (targetSet)
        {
            walkTimer += Time.deltaTime;
            aiCharacter.aiPath.destination = target;
            aiCharacter.aiPath.canMove = true;
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                aiCharacter.aiPath.canMove = false;
                targetSet = false;
                walkTimer = 0f;
                Debug.Log("Target reached");
                //CalculateTarget();

            }
            else if (walkTimer > 10.0f) //or is about to walk off an edge)
            {
                targetSet = false;
                aiCharacter.aiPath.canMove = false;
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundedWalk : BaseGroundedState
{
    private Vector2 targetA;
    private Vector2 targetB;
    private bool movingToB = true;
    private Vector2 currentTarget;
    private float walkDistance;
    private float walkTimer = 0f;
    public override void OnEnable()
    {
        base.OnEnable();
        aiCharacter.aiPath.enableRotation = false;
        anim.Play(gameObject.name + "_Walk");
        targetA = aiCharacter.transform.position;
        targetB = targetA + new Vector2(aiCharacter.AICharacterData.patrolDistance, 0);
        currentTarget = targetB;
        groundedAI.currentState = GroundedStates.Patrol;
        aiCharacter.aiPath.maxSpeed = aiCharacter.cachedSpeed * aiCharacter.CustomTimeScale;
    }
    public override void RunLogic()
    {
       WalkToTarget();
    }

    public void WalkToTarget()
    {
        // Prevent movement if CustomTimeScale is 0
        if (aiCharacter.CustomTimeScale == 0)
        {
            aiCharacter.aiPath.canMove = false;
            return;
        }
        walkTimer += Time.deltaTime;
        aiCharacter.aiPath.destination = currentTarget;
        aiCharacter.aiPath.canMove = true;

        FlipSprite();
        if (Vector2.Distance(aiCharacter.transform.position, currentTarget) < 0.1f)
        {
           aiCharacter.aiPath.canMove = false;
            if (movingToB)
            {
                currentTarget = targetA;
                movingToB = false;
            }
            else
            {
                currentTarget = targetB;
                movingToB = true;
            }
            StartCoroutine(PauseBeforeMoving(1.0f));
        }
    }

    private void FlipSprite()
    {
        if (currentTarget.x > transform.position.x)
        {
            aiCharacter.GetComponent<SpriteRenderer>().flipX = false; ; // Face right
            //aiCharacter.direction = 1;
        }
        else
        {
            aiCharacter.GetComponent<SpriteRenderer>().flipX = true; // Face left
            //aiCharacter.direction = -1;
        }
    }


    private IEnumerator PauseBeforeMoving(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            if (aiCharacter.CustomTimeScale > 0)
            {
                elapsed += Time.deltaTime * aiCharacter.CustomTimeScale;
            }
            yield return null;
        }
        aiCharacter.aiPath.canMove = true;
    }
}

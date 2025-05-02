using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAttack : BaseGroundedState
{

    public bool shouldAttack = false;
    public bool shouldRun = false;
    public Vector2 targetPos;
    public GameEventListener attackAnimFinishedCallback;

    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI.currentState = GroundedStates.Attack;
        aiCharacter.aiPath.maxSpeed = aiCharacter.AICharacterData.attackSpeed * aiCharacter.CustomTimeScale;
        if (aiCharacter.detectionSound != null)
        {
            MusicManager.instance.PlaySFX(aiCharacter.detectionSound,aiCharacter.transform.position);
        }
    }

    private void Start()
    {
       if(groundedAI.AttackAnimCheckNeeded == null)
        {
            Debug.LogError("AttackAnimCheckNeeded is null");
        }
        attackAnimFinishedCallback = gameObject.AddComponent<GameEventListener>();
        attackAnimFinishedCallback.Init(groundedAI.AttackAnimCheckNeeded, CheckForAttackRange);
 
        
    }
    public override void RunLogic()
    {
        DetermineDistance();

        if (shouldAttack)
        {
            anim.Play(gameObject.name + "_Attack");
            if(aiCharacter.attackSound != null)
            {
                MusicManager.instance.PlaySFX(aiCharacter.attackSound, aiCharacter.transform.position);
            }
        }
        else if (shouldRun)
        {
            MoveTowardsPlayer();
        }
    }

    public void DetermineDistance()
    {
        
        Vector2 playerPosition = groundedAI.playerPosition.position;
        float distance = Vector2.Distance(playerPosition, transform.position);
        if (playerPosition.x > transform.position.x)
        {
            groundedAI.GetComponent<SpriteRenderer>().flipX = false; 
        }
        else
        {
            groundedAI.GetComponent<SpriteRenderer>().flipX = true; 
        }

        //only the x value is important as the grounded cannot move vertically
        if (distance < groundedAI.AICharacterData.attackDistance)
        {
            if(!shouldAttack)
            {
                shouldAttack = true;
                shouldRun = false;
            }
        }
        else
        {
           if(!shouldRun)
            {
                shouldRun = true;
                shouldAttack = false;
            }
        }
    }

    public void MoveTowardsPlayer()
    {
        if (aiCharacter.CustomTimeScale == 0)
        {
            groundedAI.aiPath.canMove = false;
            return;
        }
        Vector2 playerPos = groundedAI.playerPosition.position;
        groundedAI.aiPath.destination = new Vector3(playerPos.x, transform.position.y, 0);
        groundedAI.aiPath.canMove = true;
        anim.Play(gameObject.name + "_Run");
        
        if (Vector2.Distance(transform.position, playerPos) < aiCharacter.AICharacterData.attackDistance)
        {
            groundedAI.aiPath.canMove = false;
            shouldAttack = true;
            shouldRun = false;
            anim.StopPlayback();
        }
    }

    public void CheckForAttackRange(Component sender, object data)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, groundedAI.AICharacterData.attackDistance);
        foreach (var item in hitColliders)
        {
            if (item.TryGetComponent<IDamageable>(out IDamageable dam))
            {
                //if item is not the gameobject attached to this
                if (item.gameObject != gameObject)
                {
                    dam.TakeDamage(groundedAI.AICharacterData.attackDamage);
                    Debug.Log("Dealt damage to: " + item.name);
                }
            }
        }
    }
}


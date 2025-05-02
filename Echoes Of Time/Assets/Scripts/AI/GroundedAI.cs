using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundedType
{
    Slime,
    Boar,
    BigBoar,
    GoblinAxe,
    GoblinSpear,
    GoblinRider,
    Skeleton,
    SkeletonArcher,
    SkeletonMage,
    SkeletonBoss,
    Spider

}
public enum GroundedStates
{
    None,
    Idle,
    Patrol,
    Attack,
    Flee,
    Dead    
}
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GroundedAI : AICharacter
{
   public override AITypes AIType => AITypes.Grounded;
    public abstract GroundedType GroundedType { get; }
    public GroundedStates currentState;

    public Rigidbody2D rb;

    private bool playerInRange = false;
    private Collider2D[] cols;
    public GroundedStates previousState { get; }

    public GameEvent AttackAnimCheckNeeded;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
        cols = GetComponents<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        base.Start();
        
    }

    private void Update()
    {
        //check detection radius
        ScanDetection();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ChangeState(GroundedStates newState)
    {
        
        if (currentState != newState)
        {

            //change state and add statescript 
            Destroy(currentStateScript);
            switch (newState)
            {
                case GroundedStates.Idle:
                    currentStateScript = transform.gameObject.AddComponent<GroundedIdle>();
                    break;
                case GroundedStates.Patrol:
                    currentStateScript = transform.gameObject.AddComponent<GroundedWalk>();
                    break;
                case GroundedStates.Attack:
                    
                    currentStateScript = transform.gameObject.AddComponent<GroundedAttack>();
                    break;
                case GroundedStates.Dead:
                    currentStateScript = transform.gameObject.AddComponent<DeadState>();
                    break;
            }

            if(currentStateScript == null)
            {
                Debug.LogError("State script not found");
            }

        }
    }

    public void ScanDetection()
    {
        //check stats detection radius for player 
        //if player in radius, change state to attack
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, AICharacterData.detectionRange);
        bool foundPlayer = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                
                foundPlayer = true;
                playerPosition = hitCollider.transform;
                if(!playerInRange)
                {
                    playerInRange = true;
                    ChangeState(GroundedStates.Attack);
                }
            }
        }
        if(!foundPlayer && playerInRange)
        {
            
            playerInRange = false;
            ChangeState(GroundedStates.Patrol);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AICharacterData.detectionRange);
    }

    public override void Die()
    {
        base.Die();
        ChangeState(GroundedStates.Dead);
        
    }

    public void EndOfAttackAnimCallback()
    {
        AttackAnimCheckNeeded.Announce(this);
    }
}

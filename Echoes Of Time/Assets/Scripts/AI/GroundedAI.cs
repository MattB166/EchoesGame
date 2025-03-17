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
    Flee
}
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GroundedAI : AICharacter
{
   public override AITypes AIType => AITypes.Grounded;
    public abstract GroundedType GroundedType { get; }
    public abstract GroundedStates currentState { get; }

    public Rigidbody2D rb;

    public GroundedStates previousState { get; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        //Debug.Log("Grounded AI Start");
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        base.Start();
        direction = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        //ChangeState(GroundedStates.Idle);
    }

    private void Update()
    {
        //check direction so can flip sprite
        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            direction = 1;
        }
        else if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            direction = -1;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ChangeState(GroundedStates newState)
    {
        //Debug.Log("Changing state to " + newState);
        if (currentState != newState)
        {
            //if(aiDestinationSetter.target != null)
            //aiDestinationSetter.target = null; //reset target path

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
            }

            if(currentStateScript == null)
            {
                Debug.LogError("State script not found");
            }

        }
    }
                
}

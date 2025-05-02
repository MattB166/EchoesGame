using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : GroundedAI
{
    public override GroundedType GroundedType => GroundedType.Boar;
    

    public override void Start()
    {
        
        base.Start();
        ChangeState(GroundedStates.Patrol);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    ///check for player in detection radius, and if player is in detection radius, change state to attack if immediate, or chase if not immediate. if chase, chase to 
    ////player's last known position on the same x axis, then patrol again. 
    ///
    public override void Die()
    {
        base.Die();
        Destroy(gameObject, 1.5f);
    }
}

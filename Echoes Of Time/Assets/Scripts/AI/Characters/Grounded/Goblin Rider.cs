using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRider : GroundedAI
{
    public override GroundedType GroundedType => GroundedType.GoblinRider;

    //public override GroundedStates currentState => GroundedStates.None;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ChangeState(GroundedStates.Patrol);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

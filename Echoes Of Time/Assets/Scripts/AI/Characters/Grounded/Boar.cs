using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : GroundedAI
{
    public override GroundedType GroundedType => GroundedType.Boar;
    public override GroundedStates currentState => GroundedStates.None;

    public override void Start()
    {
        //Debug.Log("Boar AI Start");
        base.Start();
        ChangeState(GroundedStates.Patrol);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

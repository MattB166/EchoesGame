using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AerialType
{
    Bat,
    Bee,
    Bird
}
public enum AerialStates
{
    Idle,
    Patrol,
    Attack,
    Flee
}
public abstract class AerialAI : AICharacter
{

    public override AITypes AIType => AITypes.Aerial;
    public abstract AerialType AerialType { get; }
    public abstract AerialStates AerialState { get; }

    public virtual void Start()
    {
        base.Start();
    }

    //public virtual void Update()
    //{
    //    base.Update();
    //}

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ChangeState(AerialStates newState)
    {
        if (AerialState == newState)
            return;
        //destroy current statescript. 

        switch (newState)
        {
            case AerialStates.Idle:
                //add component of this script type.
                break;
            case AerialStates.Patrol:
                break;
        }
    }
}

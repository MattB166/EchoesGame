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

    

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ChangeState(AerialStates newState)
    {
        if (AerialState == newState)
            return;
        

        switch (newState)
        {
            case AerialStates.Idle:
                
                break;
            case AerialStates.Patrol:
                break;
        }
    }
}

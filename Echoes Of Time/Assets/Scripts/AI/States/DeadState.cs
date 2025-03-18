using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseGroundedState
{
    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI.currentState = GroundedStates.Dead;
        anim.Play(gameObject.name + "_Die");
        aiCharacter.AIDeath.Announce(this,aiCharacter);

    }
}

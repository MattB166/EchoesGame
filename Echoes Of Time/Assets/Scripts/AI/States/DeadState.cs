using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseGroundedState
{
    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI.currentState = GroundedStates.Dead;
        if(aiCharacter.deathSound != null)
        {
            MusicManager.instance.PlaySFX(aiCharacter.deathSound, aiCharacter.transform.position);
        }
        anim.Play(gameObject.name + "_Die");
        aiCharacter.AIDeath.Announce(this,aiCharacter);

    }
}

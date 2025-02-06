using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derived because it needs to play an animation when collected. 
/// </summary>
public class CoinPickup : NonInputPickup
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Collect()
    {
        animator.Play("CoinCollection");
        
    }
}

    


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    


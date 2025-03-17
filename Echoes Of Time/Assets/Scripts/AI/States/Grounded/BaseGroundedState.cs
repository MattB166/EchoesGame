using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGroundedState : BaseState
{
    public GroundedAI groundedAI;
    public Rigidbody2D rb;

    public override void OnEnable()
    {
        base.OnEnable();
        groundedAI = GetComponent<GroundedAI>();
        rb = GetComponent<Rigidbody2D>();
        groundedAI.aiPath.canMove = false;
        if (groundedAI != null && rb != null)
        {
           Debug.Log("Base Grounded State Enabled");
        }
        else
        {
            Debug.LogError("Base Grounded State Enabled but groundedAI or rb is null");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonInputPickup : BasePickupItem
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : BaseNonPickup, IDestructable
{

    public int HitPoints;
    private int currentHits;
    public bool isDestroyed { get; private set; }

    private void Start()
    {
        currentHits = 0;
        isDestroyed = false;
    }
    public void DestroyObject()
    {
       isDestroyed = true;
    }

    public override void OnInteract()
    {
        PlayInteractSound();
        currentHits++;
        if(currentHits >= HitPoints)
        {
            DestroyObject();
        }
    }

    public void TakeDamage()
    {
        if(!isDestroyed)
        {
            currentHits++;
            if (currentHits >= HitPoints)
            {
                DestroyObject();
            }
        }
    }

    ////might need re work but this is the idea 
}

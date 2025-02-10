using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickupItem : BaseInteractableClass
{
    public ItemData itemData; //maybe remove this and just let each derived one decide their own data type. ?
    public override void OnInteract()
    {
        Collect();
        
       PlayPickupSound();
       CalculationDestructionTime();
    }

    protected abstract void Collect();
    public abstract void HandlePickup(Actions player, Inventory i);

    private void PlayPickupSound()
    {
        if (itemData.pickupSound != null)
        {

            AudioSource.PlayClipAtPoint(itemData.pickupSound, transform.position);
        }
    }

    private void CalculationDestructionTime()
    {
        if (itemData.isAnimatedOnPickup)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

   


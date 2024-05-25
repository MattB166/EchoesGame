using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickupItem : BaseInteractableClass
{
    public ItemData itemData;
    public override void OnInteract()
    {
        Collect();
        //Debug.Log("Collected " + itemData.itemName);
        PlayPickupSound();
       CalculationDestructionTime();
    }

    protected abstract void Collect();

  private void PlayPickupSound()
    {
       if (itemData.pickupSound != null)
        {
            
            AudioSource.PlayClipAtPoint(itemData.pickupSound, transform.position);
        }
    }

    private void CalculationDestructionTime()
    {
        if(itemData.isAnimatedOnPickup)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

   


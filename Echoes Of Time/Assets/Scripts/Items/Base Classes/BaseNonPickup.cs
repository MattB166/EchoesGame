using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNonPickup : BaseInteractableClass
{
    public NonPickupItemData itemData;
    public abstract override void OnInteract();

    protected void PlayInteractSound()
    {
        if (itemData.interactSound != null)
        {
            AudioSource.PlayClipAtPoint(itemData.interactSound, transform.position);
        }
    }
  
}

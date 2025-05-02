using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BasePickupItem : BaseInteractableClass
{
    public ItemData itemData; 
    private GameObject pickupSprite;
    private bool isEnabled = false;
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
            Destroy(this.gameObject, 0.5f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DisplayPickupPrompt()
    {
        if(transform.childCount == 0)
        {
            return;
        }
        //show image of item and text to press E to pick up
        pickupSprite = transform.GetChild(0).gameObject;
        if(pickupSprite != null && !isEnabled)
        {
            isEnabled = true;
            pickupSprite.SetActive(true);
            StartCoroutine(HidePrompt());
        }
        

    }

    public void HidePickupPrompt()
    {
        if(transform.childCount == 0)
        {
            return;
        }
        if (pickupSprite != null)
        {
            pickupSprite.SetActive(false);
            isEnabled = false;
        }
    }

    private IEnumerator HidePrompt()
    {
        yield return new WaitForSeconds(2);
        HidePickupPrompt();
    }
}

   


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseNonPickup : BaseInteractableClass
{
    public NonPickupItemData itemData;
    //maybe a static list of items in range of player so can determine which is closest to player and therefore which to interact with ?? will work in some capacity as those
    //that i want to interact with still but not through input can just not have the trigger collider and therefore not be in the list. 
    public static List<BaseNonPickup> itemsInRange = new List<BaseNonPickup>();
    //private BaseNonPickup nearestItem;
    //private Collider2D col;
    private bool playerInRange = false;

    private void Start()
    {
       // col = GetComponent<Collider2D>();
       //col.isTrigger = true;
    }
    public abstract override void OnInteract();

    protected void PlayInteractSound()
    {
        if (itemData.interactSound != null)
        {
            AudioSource.PlayClipAtPoint(itemData.interactSound, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            itemsInRange.Add(this);
            //Debug.Log("Player in range of " + itemData.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            itemsInRange.Remove(this);
            //Debug.Log("Player out of range of " + itemData.name);
        }
    }

}

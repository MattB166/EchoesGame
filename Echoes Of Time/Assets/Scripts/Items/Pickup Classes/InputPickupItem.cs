using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputPickupItem : BasePickupItem
{
    private bool playerInRange = false;
   // private InputPickupItem nearestItem;
    public static List<InputPickupItem> itemsInRange = new();
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
            //Debug.Log("Player out of range of " + itemData.name + " so has been removed from list");
        }
    }
    private void Update()
    {
        //Debug.Log("Items in range: " + itemsInRange.Count);
    }

    ////possibly remove abstract from this and just attach this to any object ?? 

}

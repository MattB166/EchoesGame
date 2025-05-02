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
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            itemsInRange.Remove(this);
          
        }
    }
    private void Update()
    {
       
    }


}

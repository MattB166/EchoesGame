using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonInputPickup : BasePickupItem
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract();
        }
    }
    protected override void Collect()
    {
        ///play particles here?? have enemy react to the pickup? 
        ///       
        Debug.Log("This is a non input pickup item. The item you have just picked up is: " + itemData.dataType.ToString());
    }
}

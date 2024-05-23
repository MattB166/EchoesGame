using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : PickupItem
{
    public float testValue;
    protected override void Collect()
    {
        Debug.Log("This item has been collected");
        Debug.Log("Test value: " + testValue);
        //can be used for weapon pickups too. enables use of the specific weapon inside player actions 
       
    }


}

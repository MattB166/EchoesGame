using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : NonInputPickup
{
    public float testValue;
    protected override void Collect()
    {
        Debug.Log("Collected " + itemData.itemName);
        Debug.Log("This is a non input pickup item");   
    }


}

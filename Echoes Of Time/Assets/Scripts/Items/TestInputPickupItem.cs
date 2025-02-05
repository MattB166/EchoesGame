using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is how you can createinput pickup item functionality. create instances of input pickup item and override the collect method to get the item data.
/// </summary>
public class TestInputPickupItem : InputPickupItem
{
    protected override void Collect()
    {
       // Debug.Log("This is an input pickup item. The item you have just picked up is: " + itemData.dataType.ToString());

    }
}

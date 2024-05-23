using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputPickupItem : InputPickupItem
{
    protected override void Collect()
    {
        Debug.Log("This is an input pickup item. The item you have just picked up is: " + itemData.name);
    }
}

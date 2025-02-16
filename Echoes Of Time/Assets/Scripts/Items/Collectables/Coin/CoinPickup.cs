using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derived because it needs to play an animation when collected. 
/// </summary>
public class CoinPickup : NonInputPickup
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Collect()
    {
        animator.Play("CoinCollection");
        
    }
    public override void HandlePickup(Actions player, Inventory i)
    {
        //Debug.Log("This is a non input pickup item. The item you have just picked up is: " + itemData.dataType.ToString());
        CoinData coinData = itemData as CoinData;
        if (coinData != null)
        {
            //Item item = new Item(); //derive into coin item to determine its use. also do not send to inventory as it is not needed there. make a bank class / economy. 
            //item.Init(coinData);
            //i.AddItem(item);
        }
    }
}

    


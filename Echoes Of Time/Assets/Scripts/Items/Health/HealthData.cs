using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Health")]
public class HealthData : ItemData
{
    public float healthAmount;
    public override void HandlePickup(Actions player)
    {
        // add health to player. removal from scene handled elsewhere.
    }
}

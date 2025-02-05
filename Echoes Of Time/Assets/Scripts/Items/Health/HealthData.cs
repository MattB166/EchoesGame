using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Health")]
public class HealthData : ItemData
{
    public int healthAmount;
    public override void HandlePickup(Actions actions)
    {
        // add health to player. removal from scene handled elsewhere.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : NonInputPickup
{
    protected override void Collect()
    {
        // Play an animation when collected
    }

    public override void HandlePickup(Actions player, Inventory i)
    {
        HealthData healthData = itemData as HealthData;
        player.AddHealth(healthData.healthAmount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupItem : InputPickupItem
{
    protected override void Collect()
    {
       // Debug.Log("This is an input pickup item. The item you have just picked up is: " + itemData.dataType.ToString());
       
    }

    public override void HandlePickup(Actions player, Inventory i)
    {

        MeleeWeaponData weaponData = itemData as MeleeWeaponData;
        if (weaponData != null)
        {
            if (!player.GetAvailableWeapons().Contains(weaponData.weaponType))
            {
                player.AddWeapon(weaponData.weaponType, weaponData.damage);
                Item item = new Item();
                item.Init(weaponData);
                i.AddItem(item);
            }
        }
        else
        {
            Debug.Log("This is not a weapon");
        }

    }
}

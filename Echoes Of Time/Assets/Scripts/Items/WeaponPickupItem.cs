using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupItem : InputPickupItem
{
    protected override void Collect()
    {
        
        
    }

    public override void HandlePickup(Actions player, Inventory i)
    {

        WeaponData weaponData = itemData as WeaponData; 
        if (weaponData != null)
        {
            if (!player.GetAvailableWeapons().Contains(weaponData.weaponType))
            {
                player.AddWeapon(weaponData.weaponType); //damages not even needed as player doesnt care about the damage of the weapon. 

                //create instance of the weapon
                GameObject weapon = weaponData.prefab;
                WeaponItem existingItem = weapon.GetComponent<WeaponItem>();
                if(existingItem == null)
                {
                    WeaponItem item = weapon.AddComponent(CheckWeaponScriptType(weaponData)) as WeaponItem;
                    if (weapon != null)
                    {
                        item.Init(weaponData, i, weapon);
                        item.SetOwner(player.gameObject);
                        i.AddItem(item);
                        ///////SAVE THE DATA HERE FOR THE SAVING SYSTEM, SO THEY ARE CONSTANTLY INITIALISED. 
                    }
                }
                else
                {
                    
                    existingItem.Init(weaponData, i, weapon);
                    existingItem.SetOwner(player.gameObject);
                    i.AddItem(existingItem);
                }
                

               

            }
        }
        else
        {
            Debug.Log("This is not a weapon");
        }

    }

    public Type CheckWeaponScriptType(WeaponData weapon)
    {
        switch(weapon.weaponType)
        {
            case Actions.Weapons.Sword:
                return typeof(SwordItem);
            case Actions.Weapons.Bow:
                return typeof(BowItem);
            case Actions.Weapons.Spear:
                return typeof(SpearItem);
            default:
                return null;
        }
    }
   
}

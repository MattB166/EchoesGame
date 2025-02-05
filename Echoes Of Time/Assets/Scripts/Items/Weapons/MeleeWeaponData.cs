using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Melee Weapon")]
public class MeleeWeaponData : ItemData //maybe change and derive into weapon data first
{
    public Actions.Weapons weaponType;
    public int damage;
    public override void HandlePickup(Actions actions) ///change to an inventory later down the line, rather than just my player actions script 
    {
        if (!actions.GetAvailableWeapons().Contains(weaponType))
        {
            actions.AddWeapon(weaponType,damage);
        }
        Debug.Log("Weapon Picked Up");
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Melee Weapon")]
public class MeleeWeaponData : ItemData //maybe change and derive into weapon data first
{
    public Actions.Weapons weaponType;
    public int damage;
    
}

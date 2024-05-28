using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
   public string weaponName;
   public float weaponDamage;
   public Sprite image;
   public bool isPrimitive;     ///customise editor to show ammo amounts if primitive is true 
   public bool ammoAmount;
   public int ammo;

    
}

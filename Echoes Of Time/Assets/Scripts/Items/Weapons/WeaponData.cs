using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
   public Actions.Weapons weaponType;
   public float weaponDamage;
   public Sprite image; 
   public int ammoAmount;
   
}

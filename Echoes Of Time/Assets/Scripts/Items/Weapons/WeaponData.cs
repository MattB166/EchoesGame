using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ranged Weapon")]
public class WeaponData : ItemData //base weapon class, melee will inherit from this as damage values required, but ranged will use this. 
{
    public Actions.Weapons weaponType;
    public GameObject prefab; 
    
    
}

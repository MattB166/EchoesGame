using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public DataType dataType;
    public Actions.Weapons weaponType;
    public string itemName;
    public Sprite itemSprite;
    public AudioClip pickupSound;
    public bool isAnimatedOnPickup;
    public float fireRate;
    public float damage;
    public int ammoAmount;
    public int healthValue;
    //public abstract void HandlePickup(int inventory);  add this, make the class abstract and keep relevant members in derived classes of item data. 

    public enum DataType
    {
        Health,
        Ammo,
        Weapon,
        Key,
        Coin,
    }

    ///AMEND THIS CLASS TO JUST CONTAIN AN ABSTRACT HANDLEPICKUP METHOD.... THEN DERIVE FROM THIS CLASS TO CREATE WEAPONDATACLASS, HEALTHDATA ETC 
}

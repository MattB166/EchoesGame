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


    public enum DataType
    {
        Health,
        Ammo,
        Weapon,
        Key,
        Coin,
    }
}

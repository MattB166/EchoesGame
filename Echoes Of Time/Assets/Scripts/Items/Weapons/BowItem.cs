using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowItem : WeaponItem
{
    //list of stored projectile types and their ammo counts. 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
        //fire the current projectile. 
    }

    public override void Init(ItemData itemData, Inventory inv, GameObject prefab)
    {
        base.Init(itemData, inv,prefab);
        weaponData = itemData as WeaponData;
        //Debug.Log("Bow initialized with " + weaponData.name);
    }

    public override void SecondaryUse()
    {
        //switch to the next projectile type. 
    }

}

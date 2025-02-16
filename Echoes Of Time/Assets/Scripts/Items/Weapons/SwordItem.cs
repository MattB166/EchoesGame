using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : MeleeWeaponItem
{    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
       // Debug.Log("Using Sword");
        
    }

    public override void Init(ItemData itemData, Inventory inv, GameObject prefab)
    {
        base.Init(itemData,inv,prefab);
        meleeWeaponData = itemData as MeleeWeaponData;
        //Debug.Log("Sword initialized with " + SwordWeaponData.name);
        //Debug.Log("Sword damage: " + SwordWeaponData.damage);
    }
}

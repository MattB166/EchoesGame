using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearItem : MeleeWeaponItem
{
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
       
    }

    public override void Init(ItemData itemData,Inventory inv)
    {
        base.Init(itemData, inv);
        meleeWeaponData = itemData as MeleeWeaponData;
        //Debug.Log("Spear initialized with " + SpearWeaponData.name);
        //Debug.Log("Spear damage: " + SpearWeaponData.damage);
    }

}

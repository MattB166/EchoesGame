using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowItem : WeaponItem
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

    public override void Init(ItemData itemData, Inventory inv)
    {
        base.Init(itemData, inv);
        weaponData = itemData as WeaponData;
        Debug.Log("Bow initialized with " + weaponData.name);
    }

}

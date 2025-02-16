using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : Item
{
    public WeaponData weaponData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init(ItemData itemData, Inventory inv)
    {
        base.Init(itemData,inv);
        weaponData = itemData as WeaponData; 
        //Debug.Log("Weapon initialized with " + weaponData.name);

    }

    public abstract override void Use(); //abstract method that will be implemented in derived classes 
   

}

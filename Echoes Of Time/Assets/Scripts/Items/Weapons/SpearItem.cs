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

    public override void SecondaryUse()
    {
        Debug.Log("Secondary use of spear");
        //on second use spear is dropped until recollected. 
        //GameObject spear = Instantiate(prefab, inventory.player.transform.position + inventory.player.transform.forward * 5.0f, inventory.player.transform.rotation);
        // inventory.RemoveItem(this); do after the spear has been thrown, otherwise the item will be removed from the inventory before it is thrown and so animation will not be played.
    }

    public override void Init(ItemData itemData,Inventory inv, GameObject prefab)
    {
        base.Init(itemData, inv,prefab);
        meleeWeaponData = itemData as MeleeWeaponData;
        //Debug.Log("Spear initialized with " + SpearWeaponData.name);
        //Debug.Log("Spear damage: " + SpearWeaponData.damage);
    }

}

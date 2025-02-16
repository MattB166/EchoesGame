using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponItem : WeaponItem
{
    public MeleeWeaponData meleeWeaponData;
    // Start is called before the first frame update
    void Start()
    {
        //player = inventory.player.transform;
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
        meleeWeaponData = itemData as MeleeWeaponData;
        //Debug.Log("Melee weapon initialized with " + meleeWeaponData.name);
        //Debug.Log("Melee weapon damage: " + meleeWeaponData.damage);
    }

    public void CheckWeaponDamage()
    { 
        Vector2 checkPoint = inventory.player.transform.position;
        Debug.Log("Checking weapon damage from weapon side");
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint, 0.6f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(meleeWeaponData.damage);
                Debug.Log("Dealt " + meleeWeaponData.damage + " damage to " + hit.name);
            }
        }
    }
}

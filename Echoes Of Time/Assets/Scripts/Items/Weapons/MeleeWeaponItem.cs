using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Melee Weapon")]
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

    public override void Init(ItemData itemData, Inventory inv, GameObject prefab)
    {
        base.Init(itemData, inv,prefab);
        meleeWeaponData = itemData as MeleeWeaponData;
        //Debug.Log("Melee weapon initialized with " + meleeWeaponData.name);
        //Debug.Log("Melee weapon damage: " + meleeWeaponData.damage);
    }

    public void CheckWeaponDamage()
    { 
        Vector2 checkPoint = ItemOwner.gameObject.transform.position;
        //Debug.Log("Checking weapon damage from weapon side");
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint, 0.6f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                if(hit.gameObject == ItemOwner)
                {
                    continue;
                }
                //Camera.main.GetComponent<CamShake>().Shake(1.0f,ShakeType.Weak);
                damageable.TakeDamage(meleeWeaponData.damage);
                GameObject impact = Instantiate(meleeWeaponData.impactAnimation, hit.transform.position, Quaternion.identity);
                Destroy(impact, 1.0f);
                //Debug.Log("Dealt " + meleeWeaponData.damage + " damage to " + hit.name);
            }
        }
    }
}

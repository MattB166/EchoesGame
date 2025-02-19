using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : NonInputPickup
{
    public override void HandlePickup(Actions player, Inventory i)
    {

        ProjectileData projectileData = itemData as ProjectileData;

        if (projectileData == null)
        {
            Debug.LogError("The item data is not a projectile data type. ");
            return;
        }
        bool hasBow = false;

        foreach (InventoryItem item in i.items)
        {
            WeaponData weaponData = item.item.itemData as WeaponData;
            if (weaponData != null && weaponData.weaponType == Actions.Weapons.Bow)
            {
                hasBow = true;
                BowItem bowItem = item.item as BowItem;
                if (bowItem != null)
                {
                    BaseProjectile projectile = projectileData.projectilePrefab.GetComponent<BaseProjectile>();
                    bowItem.AddProjectile(projectile, projectileData.pickupAmount);
                }
                break;
            }
        }


        if (!hasBow)
        {
            i.StoreProjectile(projectileData,projectileData.pickupAmount);
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

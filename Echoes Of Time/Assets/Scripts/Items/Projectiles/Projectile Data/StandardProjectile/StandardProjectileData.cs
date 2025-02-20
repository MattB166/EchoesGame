using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Data/Standard Projectile")]
public class StandardProjectileData : ProjectileData
{
   [Range(0,5)] public float damage; //the damage of the projectile

    private void OnEnable()
    {
        projectileType = ProjectileType.Standard;
    }

    private void OnValidate()
    {
        projectileType = ProjectileType.Standard;
    }
}

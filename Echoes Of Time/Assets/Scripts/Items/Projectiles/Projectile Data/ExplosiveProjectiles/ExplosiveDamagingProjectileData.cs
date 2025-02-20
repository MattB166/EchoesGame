using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Data/Explosive Damaging Projectile")]
public class ExplosiveDamagingProjectileData : ExplosiveProjectileData
{
    public float explosionDamage; //the damage of the explosion 

    private void OnEnable()
    {
        projectileType = ProjectileType.ExplodingDamaging;
    }
}

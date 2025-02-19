using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Items/Projectile Data")]
public enum ProjectileType
{
    Standard,
    Distorting,
    ExplodingDamaging,
    ExplodingDistorting,

}
public abstract class ProjectileData : ItemData
{
    public GameObject projectilePrefab; //the prefab of the projectile
    [Range(1,50)]public float projectileSpeed; //the speed of the projectile
    [Range(1,20)]public float maxDistance; //the max distance the projectile can travel
    public ProjectileType projectileType; //the type of the projectile
    [Range(1,40)]public int pickupAmount; //the amount of the projectile to pick up 
}

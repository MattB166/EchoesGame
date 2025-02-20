using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Data/Explosive Distorter Projectile")]
public class ExplosiveDistorterProjectileData : ExplosiveProjectileData
{
    public DistortionType distortionType;
    public bool timedDistortion;
    public float distortionTime;
    [HideInInspector] public float distortionValue;

    private void OnEnable()
    {
        projectileType = ProjectileType.ExplodingDistorting;
    }

    private void OnValidate()
    {
        projectileType = ProjectileType.ExplodingDistorting;
        switch(distortionType)
        {
            case DistortionType.Freeze:
                distortionValue = 0;
                break;
            case DistortionType.Half:
                distortionValue = 0.5f;
                break;
            case DistortionType.SpeedAndAHalf:
                distortionValue = 1.5f;
                break;
            case DistortionType.Double:
                distortionValue = 2;
                break;
        }
    }
}

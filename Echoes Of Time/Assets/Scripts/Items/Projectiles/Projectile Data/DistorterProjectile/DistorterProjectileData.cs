using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DistortionType
{
    Freeze,
    Half,
    SpeedAndAHalf,
    Double,
}
[CreateAssetMenu(menuName = "Items/Projectile Data/Distorter Projectile")]
public class DistorterProjectileData : ProjectileData
{
    public DistortionType distortionType;
    public bool timedDistortion; 
    public float distortionTime;
    [HideInInspector] public float distortionValue;

    private void OnEnable()
    {
        projectileType = ProjectileType.Distorting;
    }

    private void OnValidate()
    {
        projectileType = ProjectileType.Distorting;
        switch (distortionType)
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

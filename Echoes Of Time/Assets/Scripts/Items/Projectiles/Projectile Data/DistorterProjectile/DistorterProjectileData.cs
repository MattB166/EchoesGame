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
    public float distortionValue;
    public bool timedDistortion; 
    public float distortionTime;
}

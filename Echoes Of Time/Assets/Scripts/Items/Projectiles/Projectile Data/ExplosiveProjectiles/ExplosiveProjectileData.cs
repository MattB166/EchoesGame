using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveProjectileData : ProjectileData
{
    public float explosionRadius; //the radius of the explosion
    public float explosionForce; //the force of the explosion
    public float shockWaveDelay;
}

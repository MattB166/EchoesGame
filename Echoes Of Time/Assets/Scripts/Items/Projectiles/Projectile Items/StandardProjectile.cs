using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : BaseProjectile //behaviour for the standard projectile which simply does damage. 
{
    protected StandardProjectileData standardProjectileData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        CheckDistance();

    }

    //public override void Fire(Vector2 startPos)
    //{
    //    base.Fire(startPos);
        
    //}

    public override void CollisionLogic(Collider2D col)
    {
        if(col.TryGetComponent(out IDamageable damageable))
        {
            if (standardProjectileData == null)
            {
                Debug.LogError("Projectile data not set!");
            }

            damageable.TakeDamage(standardProjectileData.damage);
            Debug.Log("Standard Projectile Dealt " + standardProjectileData.damage + " damage to " + col.name);
            Destroy(gameObject); 

        }
    }

    public override void Explode()
    {
        //animations / effects in here for when it hits something it can damage. 
    }

    public override void InitialiseData(ProjectileData data)
    {
        standardProjectileData = data as StandardProjectileData; 
    }
}

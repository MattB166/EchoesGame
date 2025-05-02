using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour //common behaviour for all projectiles fired from bow.  
{
    public ProjectileData projectileData;
    protected Vector2 startingPosition;
    protected float distanceTravelled;
    protected bool grounded; //if the projectile has hit the ground after travelling max distance. 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();


        if (grounded)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Fire(Vector2 startPos) //any logic that needs to be done when the projectile is fired. distance tracking etc. 
    {
        
        startingPosition = startPos;
        
        PlayProjectileFiredSound();
    }

    /// <summary>
    /// what happens when the projectile collides with something.
    /// </summary>
    public abstract void CollisionLogic(Collider2D col);

    /// <summary>
    /// what happens after the projectile collides with something.  i.e. does it just delete or explode into particles, creates smth new?
    /// </summary>
    public abstract void Explode();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionLogic(collision);
    }

    public virtual void CheckDistance()
    {
        
        distanceTravelled = Vector2.Distance(startingPosition, transform.position);
        Vector2 direction = (transform.position - (Vector3)startingPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (distanceTravelled >= projectileData.maxDistance)
        {
            
            Destroy(gameObject, 3.0f); //destroy after 3 seconds if it has not hit anything.
            
        }
    }

    public virtual void InitialiseData(ProjectileData data)
    {
        projectileData = data;
    }

    public void PlayProjectileFiredSound()
    {
       
          if(projectileData.useSound != null)
            {
               MusicManager.instance.PlaySFX(projectileData.useSound, transform.position,0.2f);
        }
        
    }

    public void PlayProjectileHitSound()
    {
        
            if(projectileData.impactSound != null)
            {
                MusicManager.instance.PlaySFX(projectileData.impactSound, transform.position);
        }
        
    }
}

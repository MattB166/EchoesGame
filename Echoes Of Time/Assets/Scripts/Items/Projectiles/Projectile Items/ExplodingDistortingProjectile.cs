using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDistortingProjectile : BaseProjectile
{
    protected ExplosiveDistorterProjectileData explosiveDistorterProjectileData;
    int activeCoroutines = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }


    public override void CollisionLogic(Collider2D col)
    {
        if (col.TryGetComponent(out IDistortable distortable))
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosiveDistorterProjectileData.explosionRadius);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.TryGetComponent(out IDistortable distortable1))
                {

                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    float damageDelay = Mathf.Lerp(0, explosiveDistorterProjectileData.shockWaveDelay, distance / explosiveDistorterProjectileData.explosionRadius);
                    activeCoroutines++;
                    StartCoroutine(ApplyDelayedDistortion(distortable1, damageDelay));
                    
                }
            }
            Collider2D projectileCol = GetComponent<Collider2D>();
            Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (projectileCol != null && rigidbody2D != null && spriteRenderer != null)
            {
                projectileCol.enabled = false;
                rigidbody2D.velocity = Vector2.zero;
                spriteRenderer.enabled = false;

            }

        }
    }

    public override void Explode()
    {
        
    }

    private IEnumerator ApplyDelayedDistortion(IDistortable distortable, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(explosiveDistorterProjectileData.timedDistortion)
        {
            distortable.Distort(explosiveDistorterProjectileData.distortionValue, explosiveDistorterProjectileData.distortionTime);
        }
        else
        {
            distortable.Distort(explosiveDistorterProjectileData.distortionValue);
        }
        activeCoroutines--;
        if (activeCoroutines <= 0)
        {
           Destroy(gameObject);
        }
    }

    public override void InitialiseData(ProjectileData data)
    {
        explosiveDistorterProjectileData = data as ExplosiveDistorterProjectileData;
    }
}

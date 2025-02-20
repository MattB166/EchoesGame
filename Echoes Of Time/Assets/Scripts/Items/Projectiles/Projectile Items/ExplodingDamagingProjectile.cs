using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDamagingProjectile : BaseProjectile
{
    protected ExplosiveDamagingProjectileData explosiveDamagingProjectileData;
    private int activeCoroutines = 0;

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
        if(col.TryGetComponent(out IDamageable damageable))
        {
            
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosiveDamagingProjectileData.explosionRadius);
            
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.TryGetComponent(out IDamageable damageable1))
                {
                    
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    float damageDelay = Mathf.Lerp(0, explosiveDamagingProjectileData.shockWaveDelay, distance / explosiveDamagingProjectileData.explosionRadius);
                    activeCoroutines++;
                    StartCoroutine(ApplyDelayedDamage(damageable1, damageDelay));
                    Rigidbody2D rb = collider.TryGetComponent(out Rigidbody2D rb1) ? rb1 : null;
                    if (rb != null)
                    {
                        
                        Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
                        rb.AddForce(direction * explosiveDamagingProjectileData.explosionForce, ForceMode2D.Impulse);
                    }
                    

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

    private IEnumerator ApplyDelayedDamage(IDamageable damageable, float delay)
    {
       
        Debug.Log("Delay is: " + delay);
        yield return new WaitForSeconds(delay);
        damageable.TakeDamage(explosiveDamagingProjectileData.explosionDamage);
        activeCoroutines--;
        if (activeCoroutines <= 0)
        {
            Destroy(gameObject);
        }

    }

    public override void Explode()
    {
        
    }



    private void OnDrawGizmos()
    {
        if(explosiveDamagingProjectileData == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveDamagingProjectileData.explosionRadius);
    }

    public override void InitialiseData(ProjectileData data)
    {
        explosiveDamagingProjectileData = data as ExplosiveDamagingProjectileData;
    }
}

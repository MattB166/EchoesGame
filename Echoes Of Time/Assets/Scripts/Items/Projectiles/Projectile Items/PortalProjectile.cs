using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : BaseProjectile
{
    protected PortalProjectileData portalProjectileData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       CheckDistance();
    }

    public override void CheckDistance()
    {
        
        distanceTravelled = Vector2.Distance(startingPosition, transform.position);
        Vector2 direction = (transform.position - (Vector3)startingPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (distanceTravelled >= projectileData.maxDistance)
        {
            //allow a portal to be placed at the current position.
            GameObject portal = Instantiate(portalProjectileData.portalPrefab, transform.position, Quaternion.identity);
            portal.GetComponent<Portal>().openPortal = true;
            Destroy(gameObject);
        }
    }

    public override void InitialiseData(ProjectileData data)
    {
        portalProjectileData = data as PortalProjectileData;
    }

    public override void CollisionLogic(Collider2D col)
    {
       
    }

    public override void Explode()
    {
        
    }
}

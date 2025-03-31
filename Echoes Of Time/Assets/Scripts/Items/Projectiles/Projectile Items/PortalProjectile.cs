using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalProjectile : BaseProjectile
{
    protected PortalProjectileData portalProjectileData;
    public LayerMask obstacleLayer;
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
            if(CanPlacePortal())
            {
                CreatePortals();
            }
           
            Destroy(gameObject);
        }
    }

    public bool CanPlacePortal()
    {
        ///checks whether any objects are in the way of the portal being placed. walls, enemies, etc.
        float radius = 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, obstacleLayer);
        if (colliders.Length > 0)
        {
            return false;
        }
        return true;
    }


    public void CreatePortals()
    {
        //allow a portal to be placed at the current position.
        GameObject portal = Instantiate(portalProjectileData.portalPrefab, transform.position, Quaternion.identity);
        //initialise start of portal. 
        if (portal.TryGetComponent(out Portal portalScript))
        {
            portalScript.InitialisePortal(portalProjectileData.portalData, PortalNode.Start);

            GameObject EndPortal = Instantiate(portalProjectileData.portalPrefab, transform.position, Quaternion.identity);
            if (EndPortal.TryGetComponent(out Portal endPortalScript))
            {
                endPortalScript.InitialisePortal(portalProjectileData.portalData, PortalNode.End);
                portalScript.SetLinkedPortal(EndPortal);
                endPortalScript.SetLinkedPortal(portal);

            }

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

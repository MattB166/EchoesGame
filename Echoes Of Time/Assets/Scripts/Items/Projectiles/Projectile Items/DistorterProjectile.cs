using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistorterProjectile : BaseProjectile
{
    protected DistorterProjectileData distorterProjectileData;

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
        if(col.TryGetComponent(out IDistortable distortable))
        {
           //Debug.Log("hit a distortable object");
            if(distorterProjectileData.timedDistortion)
            {
                distortable.Distort(distorterProjectileData.distortionValue, distorterProjectileData.distortionTime);
            }
            else
            {
                distortable.Distort(distorterProjectileData.distortionValue);
            }
            Destroy(gameObject);
        }
    }

    public override void Explode()
    {
        
    }

    public override void InitialiseData(ProjectileData data)
    {
        distorterProjectileData = data as DistorterProjectileData;
    }
}

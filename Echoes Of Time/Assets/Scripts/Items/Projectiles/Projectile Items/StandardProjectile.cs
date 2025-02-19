using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : BaseProjectile //behaviour for the standard projectile which simply does damage. 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fire()
    {
        Debug.Log("Standard Projectile fired!");
    }
}

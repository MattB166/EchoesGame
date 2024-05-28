using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : DestructableObject
{
    private void Start()
    {
        HitPoints = 4;
        Initialise();
    }
    public override void OnInteract()
    {
        Debug.Log("interacted with crate"); 
    }

    public override void Initialise()   ////positions have to be initialised in derived classes 
    {
        originalPos = gameObject.transform.position;
        //Debug.Log(originalPos);
    }
}

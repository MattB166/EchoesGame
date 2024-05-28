using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : DestructableObject
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
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

    private void Update()
    {
        base.Update();
        if(isDestroyed)
        {
            animator.Play("Crate");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

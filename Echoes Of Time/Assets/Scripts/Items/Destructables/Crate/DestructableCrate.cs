using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructableCrate : DestructableObject
{
    Animator animator;
    private Rigidbody2D rb;


    private void Start()
    {
        animator = GetComponent<Animator>();
        Initialise();
    }
    public override void OnInteract()
    {
        
    }

    public override void Initialise()   ////positions have to be initialised in derived classes 
    {
        originalPos = gameObject.transform.position;
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : DestructableObject
{
    Animator animator;
    private Rigidbody2D rb;
    private GameObject heldObject;
   
 
    private void Start()
    {
        animator = GetComponent<Animator>();
        Initialise();
        heldObject = CollectableContainer.instance.GetRandomItem();
        Debug.Log(heldObject.name);
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

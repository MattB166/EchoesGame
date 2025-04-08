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
        //Debug.Log("interacted with crate"); 
    }

    public override void Initialise()   ////positions have to be initialised in derived classes 
    {
        originalPos = gameObject.transform.position;
        audioSource = transform.AddComponent<AudioSource>();
        if (audioSource != null)
        {
            Debug.Log("Audio source added to crate");
        }
        else
        {
            Debug.Log("Audio source not added to crate");
        }
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

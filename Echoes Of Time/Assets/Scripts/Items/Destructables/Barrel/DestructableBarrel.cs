using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestructableBarrel : DestructableObject
{
    Animator animator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isDestroyed)
        {
            animator.Play("Barrel");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public override void Initialise()
    {
        originalPos = gameObject.transform.position;
        
    }

    public override void OnInteract()
    {
        
    }
}

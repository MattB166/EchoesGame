using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrossBow : DestructableObject
{
    Animator anim;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isDestroyed)
        {
            anim.Play("CrossBow");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrossBow : DestructableObject
{
    Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float shootDirection;
    public GameObject projectile;
    private bool needsToCheck = true;
    public bool playerDetected;
    public bool shootPlayer;
    public float shootTimer;
    public float checkInterval;
    public float shootRange;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if(sr.flipX)
        {
            shootDirection = 1;
        }
        else
        {
            shootDirection = -1;
        }
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
         
        if (needsToCheck && !shootPlayer)
        {
            CastForPlayer();
        }
        if(playerDetected && !shootPlayer)
        {
            shootPlayer = true;
            StartShooting();
        }


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


    public void StartShooting()
    {
        
        InvokeRepeating("Shoot", 0, shootTimer);
    }

    public void Shoot()
    {
        

    }
    public void CastForPlayer()
    {
        
        Ray2D ray = new Ray2D(transform.position, new Vector2(shootDirection, 0));
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, shootRange,playerLayer);
        Debug.DrawRay(ray.origin, ray.direction * shootRange, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
             
                playerDetected = true;
                
            }
        }
        //reset the check interval
        needsToCheck = false;
        StartCoroutine(ResetCheck());
    }

    private IEnumerator ResetCheck()
    {
        
        yield return new WaitForSeconds(checkInterval);
        needsToCheck = true;
    }
}

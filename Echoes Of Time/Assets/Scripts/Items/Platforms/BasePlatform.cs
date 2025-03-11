using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasePlatform : MonoBehaviour, IDistortable //move all common logic from fourdirectional script to this, and derive. i.e platform sticking to player. 
{
    protected Rigidbody2D rb;
    protected bool canMove;
    protected float customTimeScale;
    protected List<Rigidbody2D> carriedBodies = new List<Rigidbody2D>();
    protected Vector2 currentVel;
    public event Action<BasePlatform, float, float> OnDistort;
    public LayerMask parentable;
    public bool playerOnPlatform;

    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
        OnDistort?.Invoke(this, timeScale, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        customTimeScale = 1;
    }

    public virtual void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            collision.gameObject.transform.SetParent(transform, true);
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                carriedBodies.Add(rb);
            }
        }
            

        


        //arriedBodies.Add(collision.gameObject.GetComponent<Rigidbody2D>());


    }


    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.transform.SetParent(null);
            carriedBodies.Remove(collision.gameObject.GetComponent<Rigidbody2D>());

        }
    }

    //private IEnumerator DelayedChangeOfParent(GameObject item)
    //{
    //    yield return null;
    //    Rigidbody2D itemRB = item.GetComponent<Rigidbody2D>();
    //    if (itemRB != null)
    //    {
    //        carriedBodies.Remove(itemRB);
    //        itemRB.velocity = new Vector2(itemRB.velocity.x, itemRB.velocity.y);
    //       item.transform.SetParent(null);
    //    }
    //}

    public void Distort(float timeScale, float time)
    {
        customTimeScale = timeScale;
        OnDistort?.Invoke(this, timeScale, time);
        StartCoroutine(ResetTimeScale(time));
    }

    protected virtual IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        customTimeScale = 1;



    }
}

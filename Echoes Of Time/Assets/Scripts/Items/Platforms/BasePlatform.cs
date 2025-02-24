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
    public event Action<BasePlatform,float, float> OnDistort;
    
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
        OnDistort?.Invoke(this,timeScale, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        customTimeScale = 1;
    }

    protected virtual void FixedUpdate()
    {
        //Debug.Log(carriedBodies.Count);
        foreach (Rigidbody2D carrierRB in carriedBodies)
        {
            Debug.Log(carrierRB.gameObject.name);
            //preserve existing velocity
            Vector2 carrierVel = carrierRB.velocity;

            if (carrierVel.y <= 0 || Mathf.Sign(currentVel.y) != Mathf.Sign(carrierVel.y))
            {
                carrierVel.y = currentVel.y;
            }

            carrierRB.velocity = new Vector2(carrierVel.x + currentVel.x, carrierVel.y);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB != null && !carriedBodies.Contains(playerRB))
            {
                carriedBodies.Add(playerRB);
                if (playerRB.velocity.y < 0)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                    collision.gameObject.transform.SetParent(transform, true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //collision.gameObject.transform.SetParent(null);
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                //Debug.Log("Removing player");
                carriedBodies.Remove(playerRB);
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y);
                collision.gameObject.transform.SetParent(null);
            }

        }
    }

    public void Distort(float timeScale, float time)
    {
        customTimeScale = timeScale;
        OnDistort?.Invoke(this,timeScale, time);
        StartCoroutine(ResetTimeScale(time));
    }

    protected virtual IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        customTimeScale = 1;



    }
}

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
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        customTimeScale = 1;
    }

    protected virtual void FixedUpdate()
    {
        foreach (Rigidbody2D carrierRB in carriedBodies)
        {
            carrierRB.position += new Vector2(currentVel.x * Time.fixedDeltaTime, 0);
            if(rb.velocity.y != 0)
            {
                carrierRB.position += new Vector2(0, currentVel.y * Time.fixedDeltaTime);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                carriedBodies.Add(playerRB);
            }
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.parent != null && collision.transform.parent.gameObject.activeInHierarchy)
            {
                collision.gameObject.transform.SetParent(null);
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRB != null)
                {
                    carriedBodies.Remove(playerRB);
                }
            }
        }
    }

    public void Distort(float timeScale, float time)
    {
        customTimeScale = timeScale;
        StartCoroutine(ResetTimeScale(time));
    }

    protected virtual IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        customTimeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsiblePlatform : BasePlatform
{
    public float collapseDelay;
    private bool hasCollapsed;
    public bool hitsFloor;

    // Start is called before the first frame update
    void Start()
    {
        customTimeScale = 1;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasCollapsed)
            {
                StartCoroutine(Collapse());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasCollapsed)
        {
            Vector2 collisionPos = collision.gameObject.transform.position;
            Vector2 platformPos = transform.position;

            if (collisionPos.y < platformPos.y)
            {
                if(collision.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(5);
                    Debug.Log("Object hit: " + collision.gameObject.name);
                   
                }
                
            }
            Destroy(gameObject, 0.5f);
        }
    }

    private IEnumerator Collapse()
    {
        yield return new WaitForSeconds(collapseDelay / customTimeScale);
        hasCollapsed = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 2 * customTimeScale;

        if (!hitsFloor)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D col in cols)
            {
                col.enabled = false;
            }
            Destroy(gameObject, 3f);

        }
    }
}

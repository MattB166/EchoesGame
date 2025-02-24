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

    private IEnumerator Collapse()
    {
        yield return new WaitForSeconds(collapseDelay / customTimeScale);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2 * customTimeScale;
        hasCollapsed = true;
        if (hitsFloor)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
                Destroy(gameObject, 3f); 
            }
        }
    }
}

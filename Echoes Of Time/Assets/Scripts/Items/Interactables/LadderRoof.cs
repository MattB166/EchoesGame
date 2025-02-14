using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderRoof : MonoBehaviour
{
    private BoxCollider2D RoofCollider;

    // Start is called before the first frame update
    void Start()
    {
        RoofCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out Movement player);
            Bounds bounds = RoofCollider.bounds;
            Vector2 playerPosition = player.transform.position;
            if (playerPosition.y > bounds.max.y)
            {
                ToggleCollider(); 
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
           
        }
    }

    private void ToggleCollider()
    {
        RoofCollider.isTrigger = !RoofCollider.isTrigger;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : IClimbable
{

    private BoxCollider2D climbingCollider;

    // Start is called before the first frame update
    void Start()
    {
        climbingCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out Movement player);
            if (player != null)
            {
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out Movement player);
            if (player != null)
            {
                Bounds bounds = climbingCollider.bounds;
                Vector2 playerPosition = player.transform.position;
                if(playerPosition.y > bounds.max.y)
                {
                    Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
                    playerRigidbody.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                }
            }
        }
    }
}

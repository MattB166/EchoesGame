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
                
            }
        }
    }
}

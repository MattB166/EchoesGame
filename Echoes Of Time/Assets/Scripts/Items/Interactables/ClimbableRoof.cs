using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableRoof : MonoBehaviour
{
    private BoxCollider2D RoofCollider;
    private bool playerOnRoof = false;
    private bool colliderCheckNeeded = false;

    // Start is called before the first frame update
    void Start()
    {
        RoofCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Player on roof ? : " + playerOnRoof);
        
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
                playerOnRoof = true;
            }
            else
            {
                playerOnRoof = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        { 
            playerOnRoof = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnRoof = false;
            StartCoroutine(DelayedColliderCheck(collision.transform));
        }
    }

    public void CheckDescent(bool descendInput)
    {
        if (descendInput && playerOnRoof)
        {
            ToggleCollider();
            //playerOnRoof = false;
        }
    }

    private void ToggleCollider()
    {
        RoofCollider.isTrigger = !RoofCollider.isTrigger;
    }

    private IEnumerator DelayedColliderCheck(Transform player)
    {
        yield return new WaitForSeconds(1.0f);
        if(player.position.y < RoofCollider.bounds.min.y)
        {
            ToggleCollider();
        }


    }

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : BaseNonPickup
{
    private Animator anim;
    private bool collected;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnInteract()
    {
        anim.Play("Chest");
        if (!collected)
        {
            
            GameObject go = ProjectileContainer.instance.GetRandomItem();
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            if (go != null)
            {
                GameObject newItem = Instantiate(go, spawnPos, Quaternion.identity);
                Collider2D col = newItem.GetComponent<Collider2D>();
                col.enabled = false;
                Rigidbody2D rb = newItem.GetComponent<Rigidbody2D>();
                if (rb != null)
                {

                    Vector2 spawnForce = new Vector2(Random.Range(-1, 1), Random.Range(2, 4));
                    rb.AddForce(spawnForce, ForceMode2D.Impulse);

                }
                StartCoroutine(SpawnVisualisation(1.5f, col));
            }
            else
            {
                Debug.Log("No item to spawn");
            }
        }
        
    }

    public void InteractWithChest(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            OnInteract();
        }
    }

    private IEnumerator SpawnVisualisation(float time, Collider2D col)
    {
        yield return new WaitForSeconds(time);
        col.enabled = true;
        collected = true;
    }
}

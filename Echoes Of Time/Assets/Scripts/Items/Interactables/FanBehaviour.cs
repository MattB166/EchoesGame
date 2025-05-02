using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBehaviour : MonoBehaviour
{
    [SerializeField] private float liftForce = 10f;
    [SerializeField] private bool isOn = false;
    private Collider2D fanCollider;
    private FanVisual fanVisual;
    public GameEvent playerInFan;
    public GameEvent playerExitedFan;
    // Start is called before the first frame update
    void Start()
    {
        fanCollider = GetComponent<Collider2D>();
        fanVisual = GetComponentInChildren<FanVisual>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOn && collision.gameObject.CompareTag("Player"))
        {
           
            Rigidbody2D rb = collision.gameObject.TryGetComponent<Rigidbody2D>(out rb) ? rb : null;
            if (rb != null)
            {
                playerInFan.Announce(this, null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOn && collision.gameObject.CompareTag("Player"))
        {
            playerExitedFan.Announce(this, null);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOn && collision.gameObject.CompareTag("Player"))
        {
            
            Rigidbody2D rb = collision.gameObject.TryGetComponent<Rigidbody2D>(out rb) ? rb : null;
            if (rb != null)
            {
                
                if(rb.velocity.y < -15)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -15);
                }
                rb.AddForce(Vector2.up * liftForce, ForceMode2D.Force);
            }
        }
    }

    public void SetFanState(bool state)
    {
        isOn = state;
        if (fanVisual != null)
        {
            fanVisual.SetFanState(state);
        }
    }

    public void ToggleFan(Component sender, object data)
    {
        SetFanState(!isOn);
    }
}

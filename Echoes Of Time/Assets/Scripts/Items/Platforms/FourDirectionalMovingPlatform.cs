using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDirectionalMovingPlatform : MonoBehaviour, IDistortable
{
    public PlatformData data;
    private Vector2 MovementCentrePoint;
    private Vector2 targetPos;
    public List<Vector2> targetPositions;
    private int currentTargetIndex;
    private bool canMove;
    private Rigidbody2D rb;
    private Vector2 lastPos;
    private Vector2 currentVel;
    private List<Rigidbody2D> carriedBodies = new List<Rigidbody2D>();

    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }


    private void Start()
    {
        customTimeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        lastPos = rb.position;
        //set start position and first target
        MovementCentrePoint = transform.position;
        canMove = true;
        //populate movement list with target positions
        targetPositions.Add(new Vector2(MovementCentrePoint.x + data.maxDistance, MovementCentrePoint.y));
        targetPositions.Add(new Vector2(MovementCentrePoint.x - data.maxDistance, MovementCentrePoint.y));
        targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y));
        if (data.VerticalMovement)
        {
            targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y + data.maxDistance));
            targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y - data.maxDistance));
            targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y)); 
        }
        currentTargetIndex = 0;
    }

    private void FixedUpdate()
    {
        currentVel = (rb.position - lastPos) / Time.fixedDeltaTime;
        lastPos = rb.position;
        Move();
        foreach (Rigidbody2D rb in carriedBodies)
        {
            rb.position += new Vector2(currentVel.x * Time.fixedDeltaTime, 0);
        }
    }

    private void Move()
    {
        if(canMove)
        {
            Vector2 direction = (targetPositions[currentTargetIndex] - (Vector2)transform.position).normalized;
            rb.MovePosition(rb.position + direction * data.speed * Time.deltaTime * customTimeScale);
            //transform.position = Vector2.MoveTowards(transform.position, targetPositions[currentTargetIndex], data.speed * Time.deltaTime * customTimeScale);
            if (Vector2.Distance(transform.position, targetPositions[currentTargetIndex]) < 0.1f)
            {
                canMove = false;
                if (data.alwaysMoving)
                {

                    StartCoroutine(Wait());
                }

            }
        }
        
    }

   
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(data.waitTime);
        currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Count;
        canMove = true;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * data.maxDistance);
        Gizmos.DrawRay(transform.position, -transform.up * data.maxDistance);
        Gizmos.DrawRay(transform.position, transform.right * data.maxDistance);
        Gizmos.DrawRay(transform.position, -transform.right * data.maxDistance);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if(playerRB != null)
            {
                carriedBodies.Add(playerRB);
            }
            collision.gameObject.transform.SetParent(transform,true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.transform.parent != null && collision.transform.parent.gameObject.activeInHierarchy)
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

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
    }

    public void Distort(float timeScale, float time)
    {
        customTimeScale = timeScale;
        StartCoroutine(ResetTimeScale(time));
    }

    private IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        customTimeScale = 1;
    }
}
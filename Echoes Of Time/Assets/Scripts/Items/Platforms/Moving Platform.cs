using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IDistortable
{
    public PlatformData data;
    private Vector2 MovementCentrePoint;
    private Vector2 targetPos;
    public List<Vector2> targetPositions;
    private int currentTargetIndex;
    private bool canMove;

    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }


    private void Start()
    {
        customTimeScale = 1;
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
        Move();
    }

    private void Move()
    {
        if(canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPositions[currentTargetIndex], data.speed * Time.deltaTime * customTimeScale);
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
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
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
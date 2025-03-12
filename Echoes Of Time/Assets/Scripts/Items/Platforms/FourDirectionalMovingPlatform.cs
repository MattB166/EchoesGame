using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDirectionalMovingPlatform : BasePlatform
{
    public PlatformData data;
    private Vector2 MovementCentrePoint;
    private Vector2 targetPos;
    public List<Vector2> targetPositions;
    private int currentTargetIndex;
   
    private Vector2 lastPos;
    

    private void Start()
    {
        customTimeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        lastPos = rb.position;
        
        MovementCentrePoint = transform.position;
        canMove = true;
        
        targetPositions.Add(new Vector2(MovementCentrePoint.x + data.maxDistance, MovementCentrePoint.y));
        targetPositions.Add(new Vector2(MovementCentrePoint.x - data.maxDistance, MovementCentrePoint.y));
        targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y));
        targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y + data.maxDistance));
        targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y - data.maxDistance));
        targetPositions.Add(new Vector2(MovementCentrePoint.x, MovementCentrePoint.y)); 
        
        currentTargetIndex = 0;
    }

   

    public override void Update()
    {
        base.Update();
        Move();
        //Debug.Log("Custom time scale : " + customTimeScale);
    }

    private void Move()
    {
        if(canMove)
        {
            //Vector2 direction = (targetPositions[currentTargetIndex] - (Vector2)transform.position).normalized;
            //rb.MovePosition(rb.position + direction * data.speed * Time.deltaTime * customTimeScale);
            
            //if (Vector2.Distance(transform.position, targetPositions[currentTargetIndex]) < 0.1f)
            //{
            //    canMove = false;
            //    if (data.alwaysMoving)
            //    {

            //        StartCoroutine(Wait());
            //    }

            //}
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
    
}
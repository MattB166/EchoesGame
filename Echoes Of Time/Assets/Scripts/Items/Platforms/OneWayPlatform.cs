using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// A platform which can be toggled in either direction by an external switch or controller. 
/// </summary>
public enum StartDirection
{
    Left,
    Right,
    Up,
    Down
}
public class OneWayPlatform : BasePlatform //maybe have another interface so can control the movement using switches 
{
    public PlatformData data;
    private Vector2 MovementStartPoint;
    private Vector2 maxPoint;
    private Vector2 currentTargetPos;
    public StartDirection startDirection;
    private float dir;
    public bool isMoving; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        customTimeScale = 1;
        MovementStartPoint = transform.position;
        if(startDirection == StartDirection.Left)
        {
            maxPoint = new Vector2(MovementStartPoint.x - data.maxDistance, MovementStartPoint.y);
            dir = -1;
        }
        if (startDirection == StartDirection.Right)
        {
            maxPoint = new Vector2(MovementStartPoint.x + data.maxDistance, MovementStartPoint.y);
            dir = 1;
        }
        if (startDirection == StartDirection.Up)
        {
            maxPoint = new Vector2(MovementStartPoint.x, MovementStartPoint.y + data.maxDistance);
            dir = 1;
        }
        if (startDirection == StartDirection.Down)
        {
            maxPoint = new Vector2(MovementStartPoint.x, MovementStartPoint.y - data.maxDistance);
            dir = -1;
        }
        currentTargetPos = maxPoint;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        Move();
        base.FixedUpdate();
    }

    public void Move()
    {
        if(canMove)
        {
            Vector2 dir = (currentTargetPos - (Vector2)transform.position).normalized;
            rb.MovePosition(rb.position + dir * data.speed * Time.deltaTime * customTimeScale);
            //isMoving = true;
            if (Vector2.Distance(transform.position, currentTargetPos) < 0.1f)
            {
                //isMoving = false;
                canMove = false;
                InvertTarget();

            }
        }
    }

    public void InvertTarget()
    {
        if(currentTargetPos == maxPoint)
        {
            currentTargetPos = MovementStartPoint;
        }
        else
        {
            currentTargetPos = maxPoint;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.right * data.maxDistance);
       
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}

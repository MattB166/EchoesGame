using System.Collections;
using System.Collections.Generic;
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
    public bool needsSwitchingOn;
    private bool needsResetting;
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
        canMove = false;
        needsSwitchingOn = true;
        //Debug.Log("can move : " + canMove + "Needs switching on : " + needsSwitchingOn);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ResetPlatform();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
       
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if(collision.gameObject.CompareTag("Player"))
        {
            //if position isnt at intended point then player has fallen off, so reset to starting point. 
            //Vector2 currentPos = transform.position;
            //if (currentPos != MovementStartPoint)
            //{
            //    needsResetting = true;
            //}
        }    
       
    }

    private void ResetPlatform()
    {
        if(needsResetting)
        {
            transform.position = MovementStartPoint;
            currentTargetPos = maxPoint;
            isMoving = false;
            needsSwitchingOn = true;
            canMove = false;
            needsResetting = false;
        }
        

    }

    public void Move()
    {
        if(canMove && !needsSwitchingOn)
        {
            Vector2 dir = (currentTargetPos - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, currentTargetPos, data.speed * Time.deltaTime * customTimeScale);
            isMoving = true;
            if (Vector2.Distance(transform.position, currentTargetPos) < 0.1f)
            {
                needsSwitchingOn = true;
                canMove = false;
                isMoving = false;
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

    private void ToggleMovement(Component sender, object data)
    {
        canMove = !canMove;
        //Debug.Log(canMove);
    }

    public void SwitchPlatform(Component sender, object data)
    {
        needsSwitchingOn = !needsSwitchingOn;
        ToggleMovement(sender, data);
        //Debug.Log(needsSwitchingOn);

    }
}

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
public class OneWayPlatform : BasePlatform  
{
    public PlatformData data;
    private Vector2 MovementStartPoint;
    private Vector2 maxPoint;
    private Vector2 currentTargetPos;
    public StartDirection startDirection;
    private float dir;
    public bool platformActive = true;
    public bool needsInverting = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        customTimeScale = 1;
        MovementStartPoint = transform.position;
        if (startDirection == StartDirection.Left)
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
        currentTargetPos = MovementStartPoint;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
       
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            
        }

    }

    private void ResetPlatform()
    {
       
    }

    public void Move()
    {
        if(platformActive)
        {
            Vector2 dir = (currentTargetPos - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, currentTargetPos, data.speed * Time.deltaTime * customTimeScale);
        }
        

    }

    public void InvertTarget()
    {
        
        if (currentTargetPos == maxPoint)
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
       
    }

    private void ToggleMovement(Component sender, object data)
    {
       
    }

    public void SwitchPlatform(Component sender, object data)
    {
       InvertTarget();
       
    }
}

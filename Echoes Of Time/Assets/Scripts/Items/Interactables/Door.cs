using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = true;
    private bool shouldMove = false;
    private bool targetSet = false;
    public float moveSpeed = 0.1f;
    public float moveDistance = 1;
    public float moveDelay = 1;
    public float moveDirection = 1;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    public GameEvent doorMoving; 

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        if(moveDirection < 0)
        {
            endPosition = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);
        }
        else
            endPosition = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
        targetPosition = endPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && shouldMove)
        {
            //announce an event that the door is about to move. 
            if (moveDelay > 0)
            {
                moveDelay -= Time.deltaTime;
            }
            else
                MoveDoor();
        }
    }

    public void MoveDoor()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
       if(transform.position == targetPosition)
        {
            shouldMove = false;
            if(targetPosition == endPosition)
            {
                targetPosition = startPosition;
            }
            else
            {

                targetPosition = endPosition;
            }

        }
    }

    public void UnlockDoor()
    {
        doorMoving.Announce(this, null);
        locked = false;
        shouldMove = true;
    }
}

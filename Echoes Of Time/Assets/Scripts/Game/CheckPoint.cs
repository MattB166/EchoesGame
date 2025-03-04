using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic checkpoint class for 
/// </summary>
public class CheckPoint : MonoBehaviour
{
    public static int nextCheckPointID = 0;
    private Animator anim;
    public bool isActivated = false;
    private bool hasCorrected = false;
    private Collider2D col;

    public string levelName;
    public int checkPointID;

    private void Awake()
    {
        checkPointID = nextCheckPointID++;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
       if(CheckPointSystem.instance != null && CheckPointSystem.instance.CheckPointActivated(checkPointID))
        {
            ActivateCheckpoint();
        }
        Debug.Log("Checkpoint ID: " + checkPointID);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated)
        {
            anim.Play("Checkpoint");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!isActivated && !hasCorrected)
            {
                ActivateCheckpoint();
                Debug.Log("Checkpoint " + checkPointID + " in level : " + levelName + " has been activated. ");
                CheckPointSystem.instance.SetNewCheckpoint(this);
            }

        }

    }

    private void ActivateCheckpoint()
    {
        isActivated = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.49f, transform.position.z);
        hasCorrected = true;
        col.enabled = false;
        
        //tell the checkpoint manager that this checkpoint is activated.
    }
}

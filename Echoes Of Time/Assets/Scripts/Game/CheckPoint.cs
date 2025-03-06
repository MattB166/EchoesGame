using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Basic checkpoint class for 
/// </summary>
public class CheckPoint : MonoBehaviour
{
    //public static int nextCheckPointID = 0;
    private Animator anim;
    public bool isActivated = false;
    private bool hasCorrected = false;
    private Collider2D col;

    public string levelName;
    [SerializeField] private int persistentCheckPointID;
    public int checkPointID => persistentCheckPointID;

    private void Awake()
    {
        if(persistentCheckPointID == 0)
        {
            Debug.LogError("Checkpoint ID is not set for checkpoint in level: " + SceneManager.GetActiveScene().name);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        //if(CheckPointSystem.instance != null && CheckPointSystem.instance.CheckPointActivated(checkPointID))
        // {
        //     ActivateCheckpoint();
        // }
        //Debug.Log("Checkpoint ID: " + checkPointID);
        levelName = SceneManager.GetActiveScene().name;
        Debug.Log("Checkpoint " + checkPointID + " in level : " + levelName + " has been created. ");
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

    public void ActivateCheckpoint()
    {
        //small delay before it runs any further. 1 for effect, 2 for enough time for game manager to save data.
        isActivated = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.49f, transform.position.z);
        Debug.Log("Moved checkpoint up a little");
        hasCorrected = true;
        col.enabled = false;
       
    }

    public void DoNotCorrectPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.49f, transform.position.z);
    }

    private IEnumerator ActivateCheckpointRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        ActivateCheckpoint();
    }

    public void ActivateCheckPointByTimer()
    {
        StartCoroutine(ActivateCheckpointRoutine());
    }
}

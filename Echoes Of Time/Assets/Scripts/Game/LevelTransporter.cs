using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelTransporter : MonoBehaviour
{
    Collider2D col;
    public string sceneName;
    public GameEvent onPlayerCollision;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(onPlayerCollision != null)
            {
                onPlayerCollision.Announce(this,null);
            }
            //move to new scene
            //Debug.Log("Moving to new scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
            //if it is a new scene, use spawn manager to dictate where player should spawn in the new scene. 
        }
    }
}

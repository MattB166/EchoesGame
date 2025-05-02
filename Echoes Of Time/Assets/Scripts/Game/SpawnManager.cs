using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Dictionary<string, Vector3> spawnLocations = new Dictionary<string, Vector3>(); //dictionary to store spawn locations of each scene

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            spawnLocations.Add("TestingLevel", new Vector3(-52, -3, 0));
            spawnLocations.Add("PoisonPuzzleLevel", new Vector3(0, 3.7f, 0));
            spawnLocations.Add("SecondScene", new Vector3(4, 3, 0));
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Vector3 GetSpawnLocation(string sceneName)
    {
        if (spawnLocations.ContainsKey(sceneName))
        {
            
            return spawnLocations[sceneName];
        }
        else
        {
            return Vector3.zero;
        }
    }
}

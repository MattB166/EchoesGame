using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Holds a list of items which can be spawned at runtime and collected by the player
/// </summary>
[System.Serializable]
public struct ItemSpawnData
{
    public GameObject itemPrefab;
    [Tooltip("The chance of this item spawning in the world when opening a chest or container for items. Note, all items must add to 100. ")]
    public float spawnChancePercentage;
}
public class CollectableContainer : MonoBehaviour
{
    public static CollectableContainer instance;
    public List<ItemSpawnData> pickupItems = new();
    private List<GameObject> spawnPool = new();


    private void Awake()
    {
       if(instance != null)
        {
            Destroy(this);
        }
       else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
       
       InitialiseSpawnPool();
    }

  private void InitialiseSpawnPool()
    {
        spawnPool.Clear();

        foreach(var item in pickupItems)
        {
            int count = Mathf.RoundToInt(item.spawnChancePercentage);
            for(int i = 0; i < count; i++)
            {
                spawnPool.Add(item.itemPrefab);
            }
        }

        while(spawnPool.Count < 100)
        {
            int randomIndex = Random.Range(0, pickupItems.Count);
            spawnPool.Add(pickupItems[randomIndex].itemPrefab);
        }
        while(spawnPool.Count > 100)
        {
            spawnPool.RemoveAt(spawnPool.Count - 1);
        }
    }

    public void SpawnRandomCollectable(Vector3 pos, Quaternion rot)
    {
        GameObject collectable = DetermineRandomCollectable();
        if(collectable != null)
        {
            Instantiate(collectable, pos, rot);
            
        }
    }

    private GameObject DetermineRandomCollectable()
    {
        if(spawnPool.Count == 0)
        {
            //InitialiseSpawnPool();
            return null;    
        }

        int randomIndex = Random.Range(0, spawnPool.Count);
        return spawnPool[randomIndex];
    }

    public GameObject GetRandomItem()
    {
        int randomIndex = Random.Range(0, spawnPool.Count);
        return spawnPool[randomIndex];
    }
}

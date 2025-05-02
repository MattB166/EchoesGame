using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProjectileSpawnData
{
    public ProjectileData projectileType;
    public float spawnChancePercentage;
}
public class ProjectileContainer : MonoBehaviour
{
    public static ProjectileContainer instance;
    public GameObject pickupPrefab;
    public List<ProjectileSpawnData> pickupItems = new();
    private List<GameObject> spawnPool = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }

        InitialiseSpawnPool();
    }

    private void InitialiseSpawnPool()
    {
        spawnPool.Clear();

        for (int i = 0; i < pickupItems.Count; i++)
        {
            int count = Mathf.RoundToInt(pickupItems[i].spawnChancePercentage);
            for (int j = 0; j < count; j++)
            {

                GameObject newPrefab = pickupPrefab;
                ProjectileData projectileData = pickupItems[i].projectileType as ProjectileData;
                newPrefab.GetComponent<ProjectilePickup>().itemData = projectileData;
                spawnPool.Add(newPrefab);
                
            }

        }

        while (spawnPool.Count < 100)
        {
            int randomIndex = Random.Range(0, pickupItems.Count);
           
            
        }
        while (spawnPool.Count > 100)
        {
            spawnPool.RemoveAt(spawnPool.Count - 1);
        }
    }

    public GameObject GetRandomItem()
    {
        int randomIndex = Random.Range(0, spawnPool.Count);
        return spawnPool[randomIndex];
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformCarousel : MonoBehaviour
{
    public GameObject platformPrefab;
    public int platformCount;
    public float radius;
    public float speed;
    public float height;

    public List<Transform> platformTransforms = new List<Transform>();
    private List<BasePlatform> platforms = new List<BasePlatform>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < platformCount; i++)
        {
            float angle = i * (360f / platformCount);
            Vector2 position = CalculatePosition(angle);
            GameObject NewPlatform = Instantiate(platformPrefab, position, Quaternion.identity);
            platformTransforms.Add(NewPlatform.transform);
            NewPlatform.transform.SetParent(transform);
            BasePlatform basePlatform = NewPlatform.TryGetComponent(out BasePlatform baseP) ? baseP : null;
            platforms.Add(basePlatform);
            basePlatform.OnDistort -= HandlePlatformDistortions;
            basePlatform.OnDistort += HandlePlatformDistortions;
            
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RotateCarousel();

    }

    public void RotateCarousel()
    {
        foreach (Transform t in platformTransforms)
        {
            BasePlatform basePlatform = t.TryGetComponent(out BasePlatform baseP) ? baseP : null;
            float timeScale = basePlatform.CustomTimeScale;
            t.RotateAround(transform.position, Vector3.forward, speed * timeScale * Time.deltaTime);

            t.localRotation = Quaternion.identity;
        }
    }

    private Vector2 CalculatePosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(radians) * radius;
        float y = Mathf.Sin(radians) * radius;
        return new Vector2(x, y);
    }

    private void HandlePlatformDistortions(BasePlatform Sourceplatform, float timeScale, float time)
    {
       foreach(var platform in platforms)
        {
            if (platform != Sourceplatform && platform.CustomTimeScale != timeScale)
            {
                platform.Distort(timeScale,time);
            }
        }
    }

}

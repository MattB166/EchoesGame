using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    private Vector2 startPos;
    private float length;
    private GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        float distX = (cam.transform.position.x - startPos.x) * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);


        if (parallaxEffect > 0.6)
        {
            transform.position = new Vector3(startPos.x + distX, cam.transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startPos.x + distX, transform.position.y, transform.position.z);
        }

        if (movement > startPos.x + length)
        {
            
            startPos.x += length;
        }
        else if (movement < startPos.x - length)
        {
            
            startPos.x -= length;
        }
    }
}

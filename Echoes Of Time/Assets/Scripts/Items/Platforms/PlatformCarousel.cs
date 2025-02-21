using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCarousel : MonoBehaviour, IDistortable
{

    public List<GameObject> platforms; //list of platforms to move
    public float speed; //speed of movement
    public Vector2 centrePivotPoint; //centre point of the carousel movement


    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
    }
}

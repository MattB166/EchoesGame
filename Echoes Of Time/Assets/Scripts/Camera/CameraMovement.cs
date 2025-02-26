using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelBounds
{
    public float minX;
    public float maxX;
    public float maxY;
    public float minY;
}

[System.Serializable]
public class BackgroundLayer
{
    public GameObject background;
    public int orderInLayer;
    public float parallaxScale;
    
    public BackgroundLayer(GameObject bg, float scale, int order)
    {
        background = bg;
        parallaxScale = scale;
        orderInLayer = order;
        
    }
}
/// <summary>
/// manages the basic movement of the camera, including perlin noise for camera shake, and camera follow for the player 
/// </summary>
public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public float moveDelay;
    private Vector3 targetTransform;
    public Vector3 playerOffset;
    private bool canMoveX;
    private bool canMoveY;
    private Vector2 camBounds;
    public LevelBounds LevelBounds;
    public List<BackgroundLayer> backgroundParallax = new List<BackgroundLayer>();
    private List<BackgroundLayer> transformsToControl = new List<BackgroundLayer>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetTransform = player.position + playerOffset;
        transform.position = targetTransform;
        CalculateCameraLevelBounds();
        InitialiseCameraBackGround();
    }

    private void LateUpdate()
    {
        CamFollow();

    }

    private void Update()
    {
       
    }
    void CamShake()
    {
        //perlin noise for camera shake 

    }

    /// <summary>
    /// generic metroidvania style camera follow for the player 
    /// </summary>
    void CamFollow()
    {

        targetTransform = player.position + playerOffset;

        Vector3 newPos = Vector3.Slerp(transform.position, targetTransform, moveDelay * Time.deltaTime);
        transform.position = ClampPositionIntoLevel(newPos);
        BackgroundMovement();

    }

    /// <summary>
    /// used to stop camera moving if player next to a wall, so player is no longer locked onto centre of cam in this case. 
    /// </summary>
    private void CalculateCameraLevelBounds()
    {

        Camera cam = GetComponent<Camera>();
        if (cam.orthographic)
        {
            float height = cam.orthographicSize;
            float width = height * cam.aspect;
            camBounds = new Vector2(width, height);
        }
    }

    private Vector3 ClampPositionIntoLevel(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, LevelBounds.minX + camBounds.x, LevelBounds.maxX - camBounds.x);
        float clampedY = Mathf.Clamp(position.y, LevelBounds.minY + camBounds.y, LevelBounds.maxY - camBounds.y);
        return new Vector3(clampedX, clampedY, position.z);
    }

    private void BackgroundMovement()
    {
        foreach (BackgroundLayer bg in transformsToControl)
        {
            if(bg.background != null)
            {
                Vector3 bgPos = bg.background.transform.position;
                bgPos.x = transform.position.x * (1 - bg.parallaxScale);
                bgPos.y = transform.position.y;
                bg.background.transform.position = bgPos;
            }
            
        }

        
    }


   

    private void InitialiseCameraBackGround()
    {
        for(int i = 0; i < backgroundParallax.Count;i++)
        {
            BackgroundLayer bg = backgroundParallax[i];
            GameObject newBG = Instantiate(bg.background, bg.background.transform.position, Quaternion.identity);
            newBG.GetComponent<SpriteRenderer>().sortingOrder = bg.orderInLayer;
            transformsToControl.Add(new BackgroundLayer(newBG, bg.parallaxScale, bg.orderInLayer));
            Debug.Log("Backgrounds initialised");
        }
        
    }
}

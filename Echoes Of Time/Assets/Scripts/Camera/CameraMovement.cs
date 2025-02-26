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
public struct BackgroundParallax
{
    public GameObject background;
    public float parallaxScale;
    public bool hasSpawnedLeft;
    public bool hasSpawnedRight;
    public BackgroundParallax(GameObject bg, float scale/*, bool LN, bool RN*/)
    {
        background = bg;
        parallaxScale = scale;
        hasSpawnedLeft = false;
        hasSpawnedRight = false;
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
    public List<BackgroundParallax> backgroundParallax = new List<BackgroundParallax>();
    //private List<BackgroundParallax> backgroundsToRemove = new List<BackgroundParallax>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetTransform = player.position + playerOffset;
        transform.position = targetTransform;
        CalculateCameraLevelBounds();
    }

    private void LateUpdate()
    {
        CamFollow();

    }

    private void Update()
    {
        CheckBackGroundBounds();
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
        foreach (BackgroundParallax bg in backgroundParallax)
        {
            Vector3 bgPos = bg.background.transform.position;
            bgPos.x = transform.position.x * (1 - bg.parallaxScale);
            bgPos.y = transform.position.y;
            bg.background.transform.position = bgPos;
        }
    }


    private void CheckBackGroundBounds()
    {

        for(int i = 0; i < backgroundParallax.Count; i++)
        {
            BackgroundParallax bg = backgroundParallax[i];
            float bgMinX = bg.background.GetComponent<SpriteRenderer>().bounds.min.x;
            float bgMaxX = bg.background.GetComponent<SpriteRenderer>().bounds.max.x;
            float camMinX = transform.position.x - camBounds.x;
            float camMaxX = transform.position.x + camBounds.x;
            
            if (camMinX <= bgMinX && !bg.hasSpawnedLeft)
            {
                Debug.Log(bg.background.name + " Camera hit left bounds");
                bg.hasSpawnedLeft = true;
                backgroundParallax[i] = bg;
                //need to spawn just one more background to the left of this type, and give its "spawned" bool the value of true for the opposite side. 
                


            }
            if (camMaxX >= bgMaxX && !bg.hasSpawnedRight)
            {
                Debug.Log(bg.background.name + " Camera hit right bounds");
                bg.hasSpawnedRight = true;
                backgroundParallax[i] = bg;
                //need to spawn just one more background to the right of this type
            }

        }
    }

    private void CleanupBackGrounds()
    {
        //check through all backgrounds in list, if they are outside of the camera bounds, destroy them, and amend their neighbours accordingly.
    }


}

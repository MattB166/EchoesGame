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


/// <summary>
/// manages the basic movement of the camera, including perlin noise for camera shake, and camera follow for the player 
/// </summary>
public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public float moveDelay;
    private Vector3 targetVector3;
    public Vector3 playerOffset;
    private bool canMoveX;
    private bool canMoveY;
    private Vector2 camBounds;
    public LevelBounds LevelBounds;
    private bool targetChanged = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetVector3 = player.position + playerOffset;
        transform.position = targetVector3;
        CalculateCameraLevelBounds();
        //InitialiseCameraBackGround();
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

        //targetVector3 = player.position + playerOffset;
        targetVector3 = GetTarget();

        Vector3 newPos = Vector3.Slerp(transform.position, targetVector3, moveDelay * Time.deltaTime);
        transform.position = ClampPositionIntoLevel(newPos);

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

    public void ChangeTarget(Component sender, object data)
    {
        Debug.Log(sender.gameObject.transform.position);
        targetChanged = true;
        targetVector3 = sender.gameObject.transform.position + playerOffset;
        StartCoroutine(ResetTarget(3));
    }


    private IEnumerator ResetTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        targetChanged = false;
    }

    public Vector3 GetTarget()
    {
        if(targetChanged)
        {
            return targetVector3;
        }
        else return player.position + playerOffset; 
    }
}

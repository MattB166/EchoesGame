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
    public float delayForTargetReset;
    public float directionFacingOffset;
    private float currentDirection;
    private Vector3 targetVector3;
    public Vector3 playerOffset;
    private Vector3 initialOffset;
    private bool canMoveX;
    private bool canMoveY;
    private Vector2 camBounds;
    public LevelBounds LevelBounds;
    private bool targetChanged = false;
    public GameEvent temporaryDifferentTarget;
    public GameEvent playerTargetAgain;
    public bool shouldLock = false;

    private Coroutine boundsTransitionCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetVector3 = player.position + playerOffset;
        initialOffset = playerOffset;
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
        if (shouldLock)
        {
            return;
        }
        //targetVector3 = player.position + playerOffset;
        targetVector3 = GetTarget();

        Vector3 newPos = Vector3.Slerp(transform.position, targetVector3, moveDelay * Time.deltaTime);
        transform.position = ClampPositionIntoLevel(newPos);
        //Debug.Log("Camera position: " + transform.position);
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
        temporaryDifferentTarget.Announce(this, delayForTargetReset);
        //Debug.Log(sender.gameObject.transform.position);
        targetChanged = true;
        Vector3 newOffset = new Vector3(0, 0, playerOffset.z);
        targetVector3 = sender.gameObject.transform.position + newOffset;

        StartCoroutine(ResetTarget(delayForTargetReset));
    }


    public void LedgeOffset(Component sender, object data)
    {
        playerOffset = new Vector3(playerOffset.x, playerOffset.y - 3, playerOffset.z);
    }

    public void ResetOffset(Component sender, object data)
    {
        //reset the offset to how it was before but taking direction facing into account
        playerOffset = new Vector3(initialOffset.x + (currentDirection * directionFacingOffset), initialOffset.y, initialOffset.z);
        //Debug.Log("Resetted player offset: " + playerOffset);
    }

    public void InvertFacingOffset(Component sender, object data)
    {
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        float dataDirection = (float)data;
        currentDirection = dataDirection;
        if (dataDirection < 0)
        {
            //new offset moves camera slightly in the facing direction of the player
            playerOffset = new Vector3(initialOffset.x - directionFacingOffset, playerOffset.y, playerOffset.z);

        }
        else if (dataDirection > 0)
        {
            playerOffset = new Vector3(initialOffset.x + directionFacingOffset, playerOffset.y, playerOffset.z);

        }
        //Debug.Log("New offset has been set: " + playerOffset);
    }

    private IEnumerator ResetTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        targetChanged = false;
        playerTargetAgain.Announce(this, null);
    }

    public Vector3 GetTarget()
    {
        if (targetChanged)
        {
            return targetVector3;
        }
        else return player.position + playerOffset;
    }

    public void ToggleCameraLock(Component sender, object data)
    {
        shouldLock = !shouldLock;
        if (shouldLock)
        {
            transform.position = ClampPositionIntoLevel(transform.position);
        }
    }

    public void AmendLevelBounds(Component sender, object data)
    {
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        if(boundsTransitionCoroutine != null)
        {
            StopCoroutine(boundsTransitionCoroutine);
        }
        boundsTransitionCoroutine = StartCoroutine(BoundsTransition((LevelBounds)data, 1.5f));

    }

    private IEnumerator BoundsTransition(LevelBounds targetBounds, float duration)
    {
        //ToggleCameraLock(this, null);
        //Debug.Log("Bounds transition started");
        float elapsedTime = 0f;
        LevelBounds initialBounds = LevelBounds;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            LevelBounds = new LevelBounds
            {
                minX = Mathf.Lerp(initialBounds.minX, targetBounds.minX, elapsedTime / duration),
                maxX = Mathf.Lerp(initialBounds.maxX, targetBounds.maxX, elapsedTime / duration),
                minY = Mathf.Lerp(initialBounds.minY, targetBounds.minY, elapsedTime / duration),
                maxY = Mathf.Lerp(initialBounds.maxY, targetBounds.maxY, elapsedTime / duration)
            };
             yield return null;
        }
        LevelBounds = targetBounds;
        //ToggleCameraLock(this, null);
        //Debug.Log("New Bounds : " + LevelBounds.minX + " " + LevelBounds.maxX + " " + LevelBounds.minY + " " + LevelBounds.maxY);
    }
}

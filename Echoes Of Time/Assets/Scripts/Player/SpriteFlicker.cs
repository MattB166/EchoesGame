using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// flicker indicates that the player is taking damage
/// </summary>
public class SpriteFlicker : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public float flickerInterval = 0.1f;
    public float flickerDuration = 1f;
    public bool alreadyFlickering = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlickerSprite(Component sender, object data)
    {
        //if already flickering, return
        if (alreadyFlickering)
        {
            return;
        }
        StartCoroutine(Flicker(flickerDuration));
    }

    private IEnumerator Flicker(float time)
    {
        alreadyFlickering = true;
        //flicker the sprite visibility for the duration
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            Debug.Log("Flicker");
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerInterval);
            elapsedTime += flickerInterval;
        }
        Debug.Log("Flicker end");
        spriteRenderer.enabled = true;
        alreadyFlickering = false;
        yield return null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerEffect : MonoBehaviour
{
    private Light2D light2D;
    [Tooltip("How often the light will flicker")]
    public float FlickerRate; //How Often the light flickers in seconds
    [Tooltip("How long the light flickers in seconds")]
    public float flickerDuration; //How long the light flickers in seconds
    private float flickerTimer;
    private float flickerDurationTimer;
    public float minIntensity;
    public float maxIntensity;
    private float currentIntensity;
    private bool isFlickering;

    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponentInChildren<Light2D>() != null ? GetComponentInChildren<Light2D>() : GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogError("Light2D not found");
        }
        flickerTimer = 0;
        flickerDurationTimer = 0;
        isFlickering = false;
    }

    // Update is called once per frame
    void Update()
    {
        flickerTimer += Time.deltaTime;
        if(flickerTimer >= FlickerRate && !isFlickering)
        {
            StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker()
    {
        isFlickering = true;
        flickerDurationTimer = 0;
        while (flickerDurationTimer < flickerDuration)
        {
            currentIntensity = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = currentIntensity;
            flickerDurationTimer += Time.deltaTime;
            yield return null;
        }

        light2D.intensity = minIntensity;

        flickerTimer = 0;
        isFlickering = false;
    }
}

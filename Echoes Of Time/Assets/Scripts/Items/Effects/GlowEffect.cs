using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowEffect : MonoBehaviour
{
    private Light2D light2D;

    [Header("Settings")]
    public float MaxIntensity;
    public float MinIntensity;
    public float SpeedToChange;
    private float currentIntensity;
    bool isIncreasing = true;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        if(light2D == null)
        {
            Debug.LogError("Light2D not found");
        }
        light2D.intensity = MinIntensity;
        currentIntensity = light2D.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        ManageIntensity();
    }

    public void IncreaseIntensity()
    {
        Debug.Log("Increasing intensity");
        currentIntensity += SpeedToChange * Time.deltaTime;
        light2D.intensity = currentIntensity;
    }

    public void DecreaseIntensity()
    {
        Debug.Log("Decreasing intensity");
        currentIntensity -= SpeedToChange * Time.deltaTime;
        light2D.intensity = currentIntensity;
    }

    public void ManageIntensity()
    {
        if (isIncreasing)
        {
            currentIntensity += SpeedToChange * Time.deltaTime;
            if (currentIntensity >= MaxIntensity)
            {
                currentIntensity = MaxIntensity;
                isIncreasing = false;
            }
        }
        else
        {
            currentIntensity -= SpeedToChange * Time.deltaTime;
            if (currentIntensity <= MinIntensity)
            {
                currentIntensity = MinIntensity;
                isIncreasing = true;
            }
        }

        light2D.intensity = currentIntensity;
    }
}

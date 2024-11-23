using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class InteractableLight : BaseNonPickup ///interactable light class within the scene where player can interact with the light source 
{
    private Light2D light2D;
    public float startingIntensity;
    public float minIntensity;
    public float maxIntensity;
    private float currentIntensity;
    private float targetIntensity;
    private bool intensityChanged;
    public override void OnInteract()
    {
        Debug.Log("Interacted with light");
        //change light state here 
    }

    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponentInChildren<Light2D>() != null ? GetComponentInChildren<Light2D>() : GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(intensityChanged)
        {
            light2D.intensity = Mathf.Lerp(currentIntensity, targetIntensity, 0.125f);
            currentIntensity = light2D.intensity;
            intensityChanged = false;
        }
    }

    public void ChangeIntensity(float val)
    {
        //change light intensity here
        intensityChanged = true;
        targetIntensity += val;
        targetIntensity = Mathf.Clamp(targetIntensity, minIntensity, maxIntensity);
        intensityChanged = false;
    }

    public void InteractWithLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           OnInteract();
        }
    }
}

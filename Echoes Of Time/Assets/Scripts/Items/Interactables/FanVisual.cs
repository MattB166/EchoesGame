using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// simple fan roation. 
/// </summary>
public class FanVisual : MonoBehaviour
{
    private bool isOn;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isOn)
        {
            transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        }
    }

    public void SetFanState(bool state)
    {
        isOn = state;
    }
}

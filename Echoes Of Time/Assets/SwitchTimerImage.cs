using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages the switch timer image when the switch is activated, it will show the tick image, and slowly fade out until it is deactivated. 
/// </summary>
public class SwitchTimerImage : MonoBehaviour
{
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;
    public float fadeSpeed = 1f;
    public float height;
    public float speed;
    public Vector3 initialPosition;
    public Vector3 finalPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        finalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + height, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    public void ActivateTimerSprite(Component sender, object data)
    {
        if (data is object[] dataArray)
        {

        }
    }
}

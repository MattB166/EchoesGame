using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupImageEffect : MonoBehaviour
{

    public Vector3 initialPos;
    public Vector3 finalPos;
    public float height;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        finalPos = new Vector3(initialPos.x, initialPos.y + height, initialPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(initialPos, finalPos, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}

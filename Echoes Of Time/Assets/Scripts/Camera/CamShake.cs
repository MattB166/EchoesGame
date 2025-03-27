using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeType
{
    Weak,
    Medium,
    Strong
}

public class CamShake : MonoBehaviour 
{
    //using perlin noise to create a camera shake effect 
    public bool shakeIsActive;
    [Range(0, 1)][SerializeField] float intensity = 0.2f; //intensity of the shake 
    [SerializeField] float intensityMultiplier = 6; //multiplier for the intensity of the shake
    [SerializeField] float intensityMagnitude = 2; //magnitude of the shake
    [SerializeField] float intensityRotationMagnitude; //rotation magnitude of the shake
    private Vector3 initialPos;
    private GameObject player;
    private Vector3 playerPos;

    float counter;

    float getPerlinFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, counter) - 0.5f) * 2;
    }

    public float Intensity
    {
        get { return intensity; }
        set { intensity = Mathf.Clamp01(value); }
    }

    public Vector2 GetVec()
    {
        return new Vector2(getPerlinFloat(0), getPerlinFloat(1));
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        initialPos = transform.position;
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //initialPos = transform.position;
        if (shakeIsActive)
        {
            counter += Time.deltaTime * Mathf.Pow(intensity,0.3f) * intensityMultiplier;
            Vector2 shake = GetVec() * intensityMagnitude;
            transform.position += new Vector3(shake.x, shake.y, 0);
        }
    }

    public void Shake(float duration)
    {
        StartCoroutine(ShakeCoroutine(duration));
    }

    public void Shake(float duration, ShakeType type)
    {
        CalculateShakeType(type);
        StartCoroutine(ShakeCoroutine(duration));
    }

    public void ShakePermanent()
    {
        shakeIsActive = true;
    }

    public void StopShake()
    {
        shakeIsActive = false;
        
    }

    IEnumerator ShakeCoroutine(float duration)
    {
        shakeIsActive = true;
        yield return new WaitForSeconds(duration);
        shakeIsActive = false;
        //transform.position = initialPos;
    }

    private void CalculateShakeType(ShakeType type)
    {
        if(type == ShakeType.Weak)
        {
            intensity = 0.1f;
            intensityMagnitude = 1;
            intensityMultiplier = 4;
        }
        else if (type == ShakeType.Medium)
        {
            intensity = 0.3f;
            intensityMagnitude = 2;
            intensityMultiplier = 6;
        }
        else if (type == ShakeType.Strong)
        {
            intensity = 0.5f;
            intensityMagnitude = 3;
            intensityMultiplier = 8;
        }
    }
}

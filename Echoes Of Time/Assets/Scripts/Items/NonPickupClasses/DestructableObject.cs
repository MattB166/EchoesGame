using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructableObject : BaseNonPickup, IDestructable
{
    public float HitPoints { get; set; }
    public bool markedForDeletion;
    protected bool isDestroyed = false;
    public abstract override void OnInteract();
    public abstract void Initialise();

    protected Vector3 originalPos;
    protected bool isShaking = false;
    protected bool isShakingRunning = false;
    private void Start()
    {
        originalPos = gameObject.transform.position;
        Debug.Log(originalPos);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Taking damage");
        HitPoints -= amount;
        OnInteract();
        isShaking = true;
        if (HitPoints <= 0)
        {
            CalculateDestruction();
        }
    }

    protected void Update()
    {
        if (isShaking && !isShakingRunning)
        {
            StartCoroutine(ShakeItem());
            isShakingRunning = true;
        }
    }

    private IEnumerator ShakeItem()
    {
       Debug.Log("Shaking");
        isShaking = true;
        float shakeDuration = 0.1f;
        float shakeAmount = 0.1f;

        float elapsedTime = 0.0f;
        while(elapsedTime < shakeDuration)
        {
           float x = Random.Range(-1, 1) * shakeAmount;
           float y = Random.Range(-1, 1) * shakeAmount;
            
           transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        transform.position = originalPos;
        isShaking = false;
        isShakingRunning = false;
    }

    public void CalculateDestruction()
    {
        if (markedForDeletion)
        {
            Destroy(gameObject);
        }
        else
            isDestroyed = true;
            DeleteAfterTime();
    }

    public void DeleteAfterTime()
    {
        Destroy(gameObject, 4.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructableObject : BaseNonPickup, IDamageable
{
    [SerializeField]
    private float hitPoints;
    public float HitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }
    public bool markedForImmediateDeletion;
    public bool spawnsItem;
    protected bool isDestroyed = false;
    private int newOrderLayer = -1;
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
        //Debug.Log("Taking damage");
        HitPoints -= amount;
        OnInteract();
        isShaking = true;
        if (HitPoints <= 0)
        {
            CalculateDestruction();
            GetComponent<SpriteRenderer>().sortingOrder = newOrderLayer;
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
        //Debug.Log("Shaking");
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
        if (markedForImmediateDeletion)
        {
            Destroy(gameObject);
        }
        else
            isDestroyed = true;
            DeleteAfterTime();

        if(spawnsItem)
        {
            GameObject go = CollectableContainer.instance.GetRandomItem();
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            if (go != null)
            {
                GameObject newItem = Instantiate(go, spawnPos, Quaternion.identity);

                Rigidbody2D rb = newItem.GetComponent<Rigidbody2D>();
                if (rb != null)
                {

                    Vector2 spawnForce = new Vector2(Random.Range(-1,1), Random.Range(2, 4));
                    rb.AddForce(spawnForce, ForceMode2D.Impulse);
                }

            }
            else
            {
                Debug.Log("No item to spawn");
            }


        }
    }

    public void DeleteAfterTime()
    {
        Destroy(gameObject, 4.0f);
    }
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AITypes
{
    Grounded,
    Aerial,
    Hybrid
}
/// <summary>
/// Defines the basic stats and behavior of an AI character.
/// </summary>
[RequireComponent(typeof(AIPath))
    , RequireComponent(typeof(AIDestinationSetter))
    , RequireComponent(typeof(Seeker)),
    RequireComponent(typeof(BoxCollider2D))]
public abstract class AICharacter : MonoBehaviour,IDamageable,IDistortable //make abstract and derive into types of AI characters. in these derived classes, the state controller will be defined, and 
                                         //the logic for changing between states will be defined.
{
    [Header("AI Character Data")]
    public AIStats AICharacterData;
    public abstract AITypes AIType { get; }
    [Header("AI Movement")]
    public AIPath aiPath;
    public AIDestinationSetter aiDestinationSetter;
    public Seeker seeker;
    public BaseState currentStateScript;
    public Transform playerPosition;

    [Header("Health")]
    [SerializeField]
    private float MaxHealth;
    public float HitPoints
    {
        get { return MaxHealth; }
        set { MaxHealth = value; }
    }

    [SerializeField]
    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }

    }
    [Header("Events")]
    public GameEvent AIDeath;

    [Header("Sounds")]
    public AudioClip detectionSound;
    public AudioClip attackSound;
    public AudioClip damagedSound;
    public AudioClip deathSound;


    //state base class given to AI character is determined by the type of AI it is. grounded/airborne/hybrid etc

    // Start is called before the first frame update
    public virtual void Start()
    {
        //initialise the AI character. 
        //Debug.Log("AI Character Start");
        Initialise();
    }

 

    public virtual void FixedUpdate()
    {
        //call fixed update logic for the current state. 
        //Debug.Log("AI Character Fixed Update");
        if (currentStateScript != null)
        {
            currentStateScript.RunLogic();
        }
    }

    /// <summary>
    /// Brings in all data from the AICharacterData object and assigns it to the AICharacter object. State Controller type(which determines
    /// what states this character can inherit, health, speed, etc.
    /// </summary>
    public virtual void Initialise()
    {
        //set up movement scripts and values
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();
        aiPath.maxSpeed = AICharacterData.moveSpeed;
        aiPath.pickNextWaypointDist = 1.2f;
        aiDestinationSetter.target = null;
        HitPoints = AICharacterData.MaxHealth;
        playerPosition = null;
        //set gravity in both the path and rigidbodies in derived classes so grounded characters can have gravity and airborne characters can have none.
    }

    public virtual void TakeDamage(float amount)
    {
        HitPoints -= amount;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //announce death and perform any inherited death logic. animation, sound, particle effects etc. 
    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
        Debug.Log("AI character distorted");
    }

    public void Distort(float timeScale, float duration)
    {
        Distort(timeScale);
        StartCoroutine(DistortionTime(duration));
    }

    private IEnumerator DistortionTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        CustomTimeScale = 1;
    }
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// poison behaviour 
/// </summary>
public class Poison : MonoBehaviour
{
    public AIDestinationSetter aiDestinationSetter;
    public GameObject player;
    public float radiusForDamage = 2f; // Radius for damage detection
    public float damageInterval;
    public float damageAmount;
    public List<IDamageable> damagedTargets = new List<IDamageable>(); // List of damageable targets
    // Start is called before the first frame update
    void Start()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (aiDestinationSetter != null && player != null)
            aiDestinationSetter.target = player.transform; // Set the target to the player transform
        else
            Debug.LogError("AIDestinationSetter or Player not found!"); // Log an error if not found

    }

    // Update is called once per frame
    void Update()
    {
        CheckForContainedDamageables();
    }

    public void CheckForContainedDamageables()
    {
        ///run check for any damageables in the radius, and perform damage over time for as long as in the radius 

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radiusForDamage);
        if(hitColliders.Length == 0)
        {
            //Debug.Log("No damageables in radius");
            return;
        }
        foreach (var item in hitColliders)
        {
            if (item.TryGetComponent<IDamageable>(out IDamageable dam))
            {
                //if item is not the gameobject attached to this
                if (item.gameObject != gameObject)
                {
                    //if not already in the list, add it to the list and start the coroutine
                    if (damagedTargets.Contains(dam))
                    {
                        //Debug.Log("Already in list");
                        continue;
                    }
                    
                    damagedTargets.Add(dam);
                    StartCoroutine(DamageOverTime(dam));
                }
            }
        }
    }

    ///enumerator which will perform damage over time for all targets in the list
    private IEnumerator DamageOverTime(IDamageable dam)
    {
        //performs the damage amount the target, at every interval. 
        //if the target is not in the list, remove it from the list
        GameObject targetObject = ((MonoBehaviour)dam).gameObject;
        while (dam != null)
        {
            if (Vector2.Distance(transform.position, targetObject.transform.position) > radiusForDamage)
            {
                damagedTargets.Remove(dam);
                yield break;
            }
            //perform damage
            dam.TakeDamage(damageAmount);
            //Debug.Log("Dealt " + damageAmount + " damage to: " + targetObject.name + " because of poison");
            yield return new WaitForSeconds(damageInterval);
        }

    }
}

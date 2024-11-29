using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INFO: This script is currently not used in the project, it is a template for a raycast to player script
// INFO: This script is intended to be used by enemies to detect the player and follow them / attack them if enemy and within range.
public class RaycastToPlayer : MonoBehaviour
{
    private GameObject holderCharacter; //INFO: type to be changed to the holder character type rather than GameObject 
    private GameObject player; //INFO: type to be changed to the player character type rather than GameObject


    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask unwalkableLayer;
    private float distanceToPlayer;

    private void Awake()
    {
        holderCharacter = GetComponent<GameObject>(); //INFO: type to be changed to the holder character type rather than GameObject
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(holderCharacter.transform.position, player.transform.position);
    }


    public bool PlayerDetected()
    {
        Ray ray = new Ray(holderCharacter.transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, unwalkableLayer))
        {
            float distToPlayer = Vector2.Distance(holderCharacter.transform.position, player.transform.position);
            if (hit.distance < distToPlayer)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                return true;
            }
            else
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                return false;
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);
            return true;
        }

        return false;
    }
}

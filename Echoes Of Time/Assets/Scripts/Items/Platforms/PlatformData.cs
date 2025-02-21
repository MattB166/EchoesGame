using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Platform Data")]
public class PlatformData : ScriptableObject
{
    public bool VerticalMovement;
    public float speed;
    public float maxDistance;
    public float waitTime;
    public bool alwaysMoving;
    
}

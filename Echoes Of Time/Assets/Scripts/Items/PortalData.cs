using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Portal Data")]
public class PortalData : ScriptableObject
{
    public float timeToReachFullSize;
    public float portalPlacementTimer;
    public float portalPlacementDistance;
    public bool oneWay;
    public bool closesAfterTwoWay;
    public float timeOpen;
    public float teleportationDelay;
    public Vector3 portalScale;
    public AudioClip portalSound;

}

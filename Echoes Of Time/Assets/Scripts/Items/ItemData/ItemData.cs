using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public AudioClip pickupSound;
    public bool isAnimatedOnPickup;
    public abstract void HandlePickup(Actions actions);

    ///AMEND THIS CLASS TO JUST CONTAIN AN ABSTRACT HANDLEPICKUP METHOD.... THEN DERIVE FROM THIS CLASS TO CREATE WEAPONDATACLASS, HEALTHDATA ETC 
}

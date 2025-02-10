using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public Sprite itemSprite;
    public AudioClip pickupSound;
    public bool isAnimatedOnPickup;


}

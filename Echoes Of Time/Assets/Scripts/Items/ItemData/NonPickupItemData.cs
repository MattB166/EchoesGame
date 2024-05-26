using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NonPickup Item Data", menuName = "Items/NonPickup Item Data")]
public class NonPickupItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public AudioClip interactSound;
}

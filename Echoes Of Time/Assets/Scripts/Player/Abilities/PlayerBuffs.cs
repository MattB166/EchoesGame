using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Buffs which can be given to player through collectables or other means, which can be used to give player temporary or permanent buffs
/// </summary>
[CreateAssetMenu(fileName = "PlayerBuff", menuName = "Player/Abilities/PlayerBuff")]
public class PlayerBuffs : ScriptableObject
{
   public EffectType MainEffectType;
   public PlayerBuffEffect playerBuffEffect;
   public GlobalBuffEffect globalBuffEffect;
   public ItemBuffEffect itemBuffEffect;
   public bool isPermanent;
   public float maxDuration;
   public float durationRemaining;
   public float buffValue;
   public bool isStackable;
   public int stackCount;
   public bool isActive;



    public enum EffectType
    {
        [Tooltip("Affects the player")]
        Player,
        [Tooltip("Affects the world")]
        Global,
        [Tooltip("Affects an item")]
        Item
    }
    public enum PlayerBuffEffect
    {
        [Tooltip("Makes the player invincible")]
        PureInvincibility,
        [Tooltip("Increases the player's damage")]
        DamageBoost,
        [Tooltip("Increases the player's speed")]
        SpeedBoost,
        [Tooltip("Increases the player's health")]
        HealthBoost

    }
    public enum GlobalBuffEffect
    {
        [Tooltip("Slows down all enemies")]
        GlobalSlowing,
        [Tooltip("Causes a global hit knock of damage")]
        GlobalDamageHit
    }

    public enum ItemBuffEffect
    {
        [Tooltip("Any fire will automatically lock on to an enemy with no aim required")]
        AutoAim,
        [Tooltip("The player will gain some health over time")]
        HealthRegen,
        [Tooltip("The player will gain an ability to travel through portals")]
        Teleportation,
        [Tooltip("The player will gain a shield which will absorb some damage")]
        Shield
    }
}

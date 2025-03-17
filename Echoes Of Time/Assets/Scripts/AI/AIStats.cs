using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AIStats")]
public class AIStats : ScriptableObject
{ 
    public string AIName;
    public float MaxHealth;
    public Sprite AIIcon;
    public float moveSpeed;
    public float attackSpeed;
    public float patrolDistance;
    public float detectionRange;
}

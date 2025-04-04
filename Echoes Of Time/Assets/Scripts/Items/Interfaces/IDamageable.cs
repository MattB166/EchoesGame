using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to mark objects that can be destroyed, such as characters and destructable objects.  
/// </summary>
public interface IDamageable
{
   float HitPoints { get; set; }
    void TakeDamage(float amount);

}

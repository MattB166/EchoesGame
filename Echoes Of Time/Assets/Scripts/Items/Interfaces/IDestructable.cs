using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to mark objects that can be destroyed, such as destructable objects like the crate. 
/// </summary>
public interface IDestructable
{
    public void DestroyObject();
}

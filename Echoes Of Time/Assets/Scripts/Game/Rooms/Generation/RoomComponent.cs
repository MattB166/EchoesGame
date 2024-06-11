using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic element of a room which is used to generate the room
/// </summary>
public abstract class RoomComponent : MonoBehaviour
{
    public abstract void Generate(Vector2 position, Transform parent);
}

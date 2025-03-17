using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base state where all grounded and aerial states will inherit from. 
/// </summary>
public abstract class BaseState : MonoBehaviour
{
    public AICharacter aiCharacter;

    public virtual void OnEnable()
    {
        aiCharacter = GetComponent<AICharacter>();
        //Debug.Log("Base State Enabled");
    }
    public virtual void RunLogic()
    {

    }
}

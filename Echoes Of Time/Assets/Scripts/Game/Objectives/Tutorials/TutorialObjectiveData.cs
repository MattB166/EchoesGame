using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    Move,
    Jump,
    Interact,
    doubleJump,
    Dash,

}
[CreateAssetMenu(menuName = "TutorialObjectiveData")]
public class TutorialObjectiveData : ObjectiveData
{
    public TutorialType tutorialType;
}

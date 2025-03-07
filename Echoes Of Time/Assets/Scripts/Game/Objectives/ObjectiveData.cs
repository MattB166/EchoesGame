using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveData")]
public class ObjectiveData : ScriptableObject
{
    // The name of the objective
    // time limited?
    // if so, how long?
    public string objectiveName;
    [TextArea(3, 10)]
    public string objectiveDescription;


}

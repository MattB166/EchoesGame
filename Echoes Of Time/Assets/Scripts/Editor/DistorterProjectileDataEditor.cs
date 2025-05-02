using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CustomEditor(typeof(DistorterProjectileData))]
public class DistorterProjectileDataEditor : ProjectileDataEditor
{

    SerializedProperty distortionType;
    SerializedProperty distortionValue;
    SerializedProperty timedDistortion;
    SerializedProperty distortionTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        distortionType = serializedObject.FindProperty("distortionType");
        distortionValue = serializedObject.FindProperty("distortionValue");
        timedDistortion = serializedObject.FindProperty("timedDistortion");
        distortionTime = serializedObject.FindProperty("distortionTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
    }


    protected override void ProjectileSpecificFields()
    {
        EditorGUILayout.PropertyField(distortionType);
        EditorGUILayout.PropertyField(timedDistortion);

        switch (distortionType.enumValueIndex)
        {
            case (int)DistortionType.Freeze:
                
                distortionValue.floatValue = 0;
                break;
            case (int)DistortionType.Half:
                
                distortionValue.floatValue = 0.5f;
                break;
            case (int)DistortionType.SpeedAndAHalf:
                
                distortionValue.floatValue = 1.5f;
                break;
            case (int)DistortionType.Double:
                
                distortionValue.floatValue = 2;
                break;
        }


        if(timedDistortion.boolValue)
        {
            EditorGUILayout.PropertyField(distortionTime);
        }
    }
}

    


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(DistorterProjectileData))]
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

    protected override void ProjectileSpecificFields()
    {
        EditorGUILayout.PropertyField(distortionType);
        EditorGUILayout.PropertyField(timedDistortion);

        switch (distortionType.enumValueIndex)
        {
            case (int)DistortionType.Freeze:
                //EditorGUILayout.HelpBox("This projectile will freeze the distortable", MessageType.Info);
                distortionValue.floatValue = 0;
                break;
            case (int)DistortionType.Half:
                //EditorGUILayout.HelpBox("This projectile will slow the distortable down by half", MessageType.Info);
                distortionValue.floatValue = 0.5f;
                break;
            case (int)DistortionType.SpeedAndAHalf:
                //EditorGUILayout.HelpBox("This projectile will speed the distortable up by 1.5x", MessageType.Info);
                distortionValue.floatValue = 1.5f;
                break;
            case (int)DistortionType.Double:
                //EditorGUILayout.HelpBox("This projectile will double the speed of the distortable", MessageType.Info);
                distortionValue.floatValue = 2;
                break;
        }


        if(timedDistortion.boolValue)
        {
            EditorGUILayout.PropertyField(distortionTime);
        }
    }
}

    


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ExplosiveDistorterProjectileEditor : ProjectileDataEditor
{
    SerializedProperty distortionType;
    SerializedProperty distortionValue;
    SerializedProperty timedDistortion;
    SerializedProperty distortionTime;
    SerializedProperty explosionRadius;
    SerializedProperty explosionForce;
    SerializedProperty shockWaveDelay;

    protected override void OnEnable()
    {
        base.OnEnable();
        distortionType = serializedObject.FindProperty("distortionType");
        distortionValue = serializedObject.FindProperty("distortionValue");
        timedDistortion = serializedObject.FindProperty("timedDistortion");
        distortionTime = serializedObject.FindProperty("distortionTime");
        explosionRadius = serializedObject.FindProperty("explosionRadius");
        explosionForce = serializedObject.FindProperty("explosionForce");
        shockWaveDelay = serializedObject.FindProperty("shockWaveDelay");

    }

    protected override void ProjectileSpecificFields()
    {
        base.ProjectileSpecificFields();
        EditorGUILayout.PropertyField(distortionType);
        EditorGUILayout.PropertyField(timedDistortion);
        EditorGUILayout.PropertyField(explosionRadius);
        EditorGUILayout.PropertyField(explosionForce);
        EditorGUILayout.PropertyField(shockWaveDelay);


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


        if (timedDistortion.boolValue)
        {
            EditorGUILayout.PropertyField(distortionTime);
        }
    }
}

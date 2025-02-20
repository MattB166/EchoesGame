using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(ExplosiveDistorterProjectile))]
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


        if (timedDistortion.boolValue)
        {
            EditorGUILayout.PropertyField(distortionTime);
        }
    }
}

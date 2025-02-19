using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(DistorterProjectileData))]
public class DistorterProjectileDataEditor : Editor
{

    SerializedProperty projectilePrefab;
    SerializedProperty projectileSpeed;
    SerializedProperty maxDistance;
    SerializedProperty projectileType;
    SerializedProperty pickupAmount;



    SerializedProperty distortionType;
    SerializedProperty distortionValue;
    SerializedProperty timedDistortion;
    SerializedProperty distortionTime;

    private void OnEnable()
    {
        projectilePrefab = serializedObject.FindProperty("projectilePrefab");
        projectileSpeed = serializedObject.FindProperty("projectileSpeed");
        maxDistance = serializedObject.FindProperty("maxDistance");
        projectileType = serializedObject.FindProperty("projectileType");
        pickupAmount = serializedObject.FindProperty("pickupAmount");


        distortionType = serializedObject.FindProperty("distortionType");
        distortionValue = serializedObject.FindProperty("distortionValue");
        timedDistortion = serializedObject.FindProperty("timedDistortion");
        distortionTime = serializedObject.FindProperty("distortionTime");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(projectilePrefab);
        EditorGUILayout.PropertyField(projectileSpeed);
        EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(projectileType);
        EditorGUILayout.PropertyField(pickupAmount);
        
        switch(projectileType.enumValueIndex)
        {
            case (int)ProjectileType.Standard:
                //EditorGUILayout.HelpBox("This projectile will not distort the distortable", MessageType.Info);
                break;
            case (int)ProjectileType.Distorting:
                EditorGUILayout.PropertyField(distortionType);
                EditorGUILayout.PropertyField(timedDistortion);
                //EditorGUILayout.HelpBox("This projectile will distort the distortable", MessageType.Info);
                break;
            case (int)ProjectileType.ExplodingDamaging:
                //EditorGUILayout.HelpBox("This projectile will explode and damage the distortable", MessageType.Info);
                break;
            case (int)ProjectileType.ExplodingDistorting:
                //EditorGUILayout.HelpBox("This projectile will explode and distort the distortable", MessageType.Info);
                
                break;
        }

        

        switch(distortionType.enumValueIndex)
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

        serializedObject.ApplyModifiedProperties();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(ProjectileData))]
public class ProjectileDataEditor : Editor
{
    SerializedProperty projectilePrefab;
    SerializedProperty projectileSpeed;
    SerializedProperty maxDistance;
    SerializedProperty projectileType;
    SerializedProperty pickupAmount;

    protected virtual void OnEnable()
    {
        projectilePrefab = serializedObject.FindProperty("projectilePrefab");
        projectileSpeed = serializedObject.FindProperty("projectileSpeed");
        maxDistance = serializedObject.FindProperty("maxDistance");
        projectileType = serializedObject.FindProperty("projectileType");
        pickupAmount = serializedObject.FindProperty("pickupAmount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(projectilePrefab);
        EditorGUILayout.PropertyField(projectileSpeed);
        EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(projectileType);
        EditorGUILayout.PropertyField(pickupAmount);
        ProjectileSpecificFields();
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void ProjectileSpecificFields()
    {

    }

   
}

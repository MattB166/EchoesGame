using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty weaponName;
    SerializedProperty weaponDamage;
    SerializedProperty image;
    SerializedProperty isPrimitive;
    SerializedProperty ammoAmount;
    SerializedProperty ammo;
    

    private void OnEnable()
    {
        weaponName = serializedObject.FindProperty("weaponName");
        image = serializedObject.FindProperty("weaponDamage");
        weaponDamage = serializedObject.FindProperty("image");
        isPrimitive = serializedObject.FindProperty("isPrimitive");
        ammoAmount = serializedObject.FindProperty("ammoAmount");
        ammo = serializedObject.FindProperty("ammo");
       
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponName);
        EditorGUILayout.PropertyField(weaponDamage);
        EditorGUILayout.PropertyField(image);
        EditorGUILayout.PropertyField(isPrimitive);

        if(isPrimitive.boolValue == false)
        {
            EditorGUILayout.PropertyField(ammoAmount);
            EditorGUILayout.PropertyField(ammo);
        }
       

        serializedObject.ApplyModifiedProperties();
    }
}

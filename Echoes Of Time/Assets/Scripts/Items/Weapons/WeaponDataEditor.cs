using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty weaponType;
    SerializedProperty weaponDamage;
    SerializedProperty image;
    SerializedProperty ammoAmount;
    

    private void OnEnable()
    {
        weaponType = serializedObject.FindProperty("weaponType");
        image = serializedObject.FindProperty("weaponDamage");
        weaponDamage = serializedObject.FindProperty("image");
        ammoAmount = serializedObject.FindProperty("ammoAmount");
      
       
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponType);
        

        switch((Actions.Weapons)weaponType.enumValueIndex)
        {
            case Actions.Weapons.Sword:
                EditorGUILayout.HelpBox("This weapon is a sword", MessageType.Info);
              
                EditorGUILayout.PropertyField(image);
                EditorGUILayout.PropertyField(weaponDamage);
                break;
            case Actions.Weapons.Spear:
                EditorGUILayout.HelpBox("This weapon is a spear", MessageType.Info);
               
                EditorGUILayout.PropertyField(image);
                EditorGUILayout.PropertyField(weaponDamage);
                break;
            case Actions.Weapons.Bow:
               
                EditorGUILayout.PropertyField(image);
                EditorGUILayout.PropertyField(ammoAmount);
                EditorGUILayout.PropertyField(weaponDamage);
                break;
            case Actions.Weapons.None:
                EditorGUILayout.HelpBox("This weapon is not a weapon", MessageType.Info);
                break;
        }   

        
        
       
        

        

        serializedObject.ApplyModifiedProperties();
    }
}

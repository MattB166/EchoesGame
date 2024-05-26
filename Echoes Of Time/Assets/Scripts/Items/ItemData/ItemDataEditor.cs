using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    SerializedProperty itemName;
    SerializedProperty itemSprite;
    SerializedProperty pickupSound;
    SerializedProperty dataType;
    SerializedProperty isAnimatedOnPickup;

    SerializedProperty fireRate;
    SerializedProperty damage;
    SerializedProperty ammoAmount;
    SerializedProperty healthValue;

    private void OnEnable()
    {
        itemName = serializedObject.FindProperty("itemName");
        itemSprite = serializedObject.FindProperty("itemSprite");
        pickupSound = serializedObject.FindProperty("pickupSound");
        dataType = serializedObject.FindProperty("dataType");
        isAnimatedOnPickup = serializedObject.FindProperty("isAnimatedOnPickup");
        

        fireRate = serializedObject.FindProperty("fireRate");
        damage = serializedObject.FindProperty("damage");
        ammoAmount = serializedObject.FindProperty("ammoAmount");
        healthValue = serializedObject.FindProperty("healthValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(itemSprite);
        EditorGUILayout.PropertyField(pickupSound);
        EditorGUILayout.PropertyField(dataType);
        EditorGUILayout.PropertyField(isAnimatedOnPickup);

        ItemData.DataType type = (ItemData.DataType)dataType.enumValueIndex;

        switch(type)
        {
            case ItemData.DataType.Health:
                EditorGUILayout.PropertyField(healthValue);
                break;
            case ItemData.DataType.Ammo:
               EditorGUILayout.PropertyField(ammoAmount);
                break;
            case ItemData.DataType.Weapon:
                EditorGUILayout.PropertyField(fireRate);
                EditorGUILayout.PropertyField(damage);
                break;
            case ItemData.DataType.Key:
                EditorGUILayout.HelpBox("This item will give the player a key.", MessageType.Info);
                break;
            case ItemData.DataType.Coin:
                EditorGUILayout.HelpBox("This item will give the player a coin.", MessageType.Info);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

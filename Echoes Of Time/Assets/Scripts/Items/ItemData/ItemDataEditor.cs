using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    SerializedProperty itemName;
    SerializedProperty weaponType;
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
        weaponType = serializedObject.FindProperty("weaponType");
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
               
                break;
            case ItemData.DataType.Weapon:
                switch((Actions.Weapons)weaponType.enumValueIndex)
                {
                    case Actions.Weapons.Sword:
                        EditorGUILayout.HelpBox("This weapon is a sword", MessageType.Info);
                        EditorGUILayout.PropertyField(damage);
                        break;
                    case Actions.Weapons.Spear:
                        EditorGUILayout.HelpBox("This weapon is a spear", MessageType.Info);
                        EditorGUILayout.PropertyField(damage);
                        break;
                    case Actions.Weapons.Bow:
                        EditorGUILayout.HelpBox("This weapon is a bow", MessageType.Info);
                        EditorGUILayout.PropertyField(fireRate);
                        EditorGUILayout.PropertyField(damage);
                        EditorGUILayout.PropertyField(ammoAmount);
                        break;
                    case Actions.Weapons.None:
                        EditorGUILayout.HelpBox("This weapon is not a weapon", MessageType.Info);
                        break;
                }
                EditorGUILayout.PropertyField(weaponType);
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

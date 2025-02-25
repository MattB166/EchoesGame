//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlayerBuffs))]
//public class CustomEditorBuffs : Editor
//{
//    SerializedProperty MainEffectType;
//    SerializedProperty playerBuffEffect;
//    SerializedProperty globalBuffEffect;
//    SerializedProperty itemBuffEffect;
//    SerializedProperty isPermanent;
//    SerializedProperty maxDuration;
//    SerializedProperty durationRemaining;
//    SerializedProperty buffValue;
//    SerializedProperty isStackable;
//    SerializedProperty stackCount;
//    SerializedProperty isActive;

//    private void OnEnable()
//    {
//        MainEffectType = serializedObject.FindProperty("MainEffectType");
//        playerBuffEffect = serializedObject.FindProperty("playerBuffEffect");
//        globalBuffEffect = serializedObject.FindProperty("globalBuffEffect");
//        itemBuffEffect = serializedObject.FindProperty("itemBuffEffect");
//        isPermanent = serializedObject.FindProperty("isPermanent");
//        maxDuration = serializedObject.FindProperty("maxDuration");
//        durationRemaining = serializedObject.FindProperty("durationRemaining");
//        buffValue = serializedObject.FindProperty("buffValue");
//        isStackable = serializedObject.FindProperty("isStackable");
//        stackCount = serializedObject.FindProperty("stackCount");
//        isActive = serializedObject.FindProperty("isActive");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(MainEffectType);

//        PlayerBuffs.EffectType type = (PlayerBuffs.EffectType)MainEffectType.enumValueIndex;

//        switch (type)
//        {
//            case PlayerBuffs.EffectType.Player:
//                EditorGUILayout.PropertyField(playerBuffEffect);
//                PlayerBuffs.PlayerBuffEffect playerEffect = (PlayerBuffs.PlayerBuffEffect)playerBuffEffect.enumValueIndex;
//                switch (playerEffect)
//                {
//                    case PlayerBuffs.PlayerBuffEffect.PureInvincibility:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.PlayerBuffEffect.DamageBoost:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(buffValue);
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.PlayerBuffEffect.SpeedBoost:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.PlayerBuffEffect.HealthBoost:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(buffValue);
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                }
//                break;
//            case PlayerBuffs.EffectType.Global:
//                EditorGUILayout.PropertyField(globalBuffEffect);
//                PlayerBuffs.GlobalBuffEffect globalEffect = (PlayerBuffs.GlobalBuffEffect)globalBuffEffect.enumValueIndex;
//                switch (globalEffect)
//                {
//                    case PlayerBuffs.GlobalBuffEffect.GlobalSlowing:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(buffValue);
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.GlobalBuffEffect.GlobalDamageHit:
//                        EditorGUILayout.PropertyField(maxDuration);
//                        EditorGUILayout.PropertyField(durationRemaining);
//                        EditorGUILayout.PropertyField(buffValue);
//                        EditorGUILayout.PropertyField(isStackable);
//                        if (isStackable.boolValue == true)
//                        {
//                            EditorGUILayout.PropertyField(stackCount);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                }
//                break;
//            case PlayerBuffs.EffectType.Item:
//                EditorGUILayout.PropertyField(itemBuffEffect);
//                PlayerBuffs.ItemBuffEffect itemEffect = (PlayerBuffs.ItemBuffEffect)itemBuffEffect.enumValueIndex;
//                switch (itemEffect)
//                {
//                    case PlayerBuffs.ItemBuffEffect.AutoAim:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.ItemBuffEffect.HealthRegen:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(buffValue);
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.ItemBuffEffect.Teleportation:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                    case PlayerBuffs.ItemBuffEffect.Shield:
//                        EditorGUILayout.PropertyField(isPermanent);
//                        if (isPermanent.boolValue == false)
//                        {
//                            EditorGUILayout.PropertyField(maxDuration);
//                            EditorGUILayout.PropertyField(durationRemaining);
//                        }
//                        EditorGUILayout.PropertyField(isActive);
//                        break;
//                }
//                break;
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}

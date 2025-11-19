// Editor extensions for the PotionGame namespace.  This file contains custom
// inspectors for design‑time assets and is compiled only in the Unity
// editor.  By isolating the editor code into its own compilation unit and
// wrapping it in an editor directive we ensure that runtime builds are not
// affected.

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace PotionGame
{
#if UNITY_EDITOR

    /// <summary>
    /// Custom inspector for DialogueNode.  Exposes fields for the speaker,
    /// text, requested potion, actions and choices.  Each choice allows
    /// designers to specify choice text, actions and the next node.  This
    /// inspector is intended to provide a simple but structured way to
    /// author branching dialogue without requiring a separate graph editor.
    /// </summary>
    [CustomEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : Editor
    {
        private ReorderableList choicesList;

        private void OnEnable()
        {
            SerializedProperty choicesProp = serializedObject.FindProperty("choices");
            choicesList = new ReorderableList(serializedObject, choicesProp, true, true, true, true);
            choicesList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Choices");
            };
            choicesList.elementHeightCallback = (int index) =>
            {
                // Provide extra height for each choice entry
                return EditorGUIUtility.singleLineHeight * 5f;
            };
            choicesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = choicesList.serializedProperty.GetArrayElementAtIndex(index);
                Rect r = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);
                // Choice text
                SerializedProperty choiceTextProp = element.FindPropertyRelative("choiceText");
                EditorGUI.PropertyField(r, choiceTextProp, new GUIContent("Choice Text"));

                // Next node reference
                r.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty nextNodeProp = element.FindPropertyRelative("nextNode");
                EditorGUI.PropertyField(r, nextNodeProp, new GUIContent("Next Node"));

                // Actions list
                r.y += EditorGUIUtility.singleLineHeight;
                SerializedProperty actionsProp = element.FindPropertyRelative("actions");
                EditorGUI.PropertyField(r, actionsProp, new GUIContent("Actions"), true);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // Speaker and text fields
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speakerName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("text"));

            // Requested potion selection: designers can assign a PotionRecipe asset directly
            SerializedProperty requestedPotionProp = serializedObject.FindProperty("requestedPotion");
            EditorGUILayout.PropertyField(requestedPotionProp, new GUIContent("Requested Potion"));

            // Node actions
            EditorGUILayout.PropertyField(serializedObject.FindProperty("actions"), new GUIContent("Node Actions"), true);

            // Choices list
            choicesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
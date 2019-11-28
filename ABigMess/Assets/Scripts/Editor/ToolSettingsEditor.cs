using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToolSettings))]
public class ToolSettingsEditor : Editor
{
    ToolSettings toolSettings;
    SerializedProperty interactionProp;

    void OnEnable()
    {
        toolSettings = (ToolSettings)target;
        interactionProp = serializedObject.FindProperty("interactionsList");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Add new interaction"))
        {
            if (toolSettings.interactionsList == null)
            {
                toolSettings.interactionsList = new List<Interaction>();
            }
            toolSettings.interactionsList.Add(new Interaction());
        }

      //  EditorGUILayout.PropertyField(serializedObject.FindProperty("interactionsList"));

        for (int i = 0; i < interactionProp.arraySize; i++)
        {
            SerializedProperty interactionListRef = interactionProp.GetArrayElementAtIndex(i);
            SerializedProperty objectTypeRef = interactionListRef.FindPropertyRelative("objectConcerned");
            AddPopup(ref objectTypeRef, "Object concerned : ", typeof(ObjectSettings.ObjectType));
            //EditorGUILayout.;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddPopup(ref SerializedProperty ourSerializedProperty, string nameOfLabel, System.Type typeOfEnum)
    {
        //ENUM POPUP

        Rect ourRect = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(ourRect, GUIContent.none, ourSerializedProperty);
        EditorGUI.BeginChangeCheck();

        int actualSelected = 1;
        int selectionFromInspector = ourSerializedProperty.intValue;
        string[] enumNamesList = System.Enum.GetNames(typeOfEnum);
        actualSelected = EditorGUILayout.Popup(nameOfLabel, selectionFromInspector, enumNamesList);
        ourSerializedProperty.intValue = actualSelected;

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }
}

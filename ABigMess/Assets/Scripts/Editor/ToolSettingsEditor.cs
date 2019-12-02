using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ToolSettings))]
public class ToolSettingsEditor : Editor
{
    ToolSettings toolSettings;
    SerializedProperty interactionProp;
    Dictionary<int, ReorderableList> eventsToLaunchList;
    Dictionary<int, bool> toDelete;

    void OnEnable()
    {
        toolSettings = (ToolSettings)target;
        interactionProp = serializedObject.FindProperty("interactionsList");
        eventsToLaunchList = new Dictionary<int, ReorderableList>();
        toDelete = new Dictionary<int, bool>();
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


        for (int i = 0; i < interactionProp.arraySize; i++)
        {
            SerializedProperty interactionListRef = interactionProp.GetArrayElementAtIndex(i);
            SerializedProperty objectTypeRef = interactionListRef.FindPropertyRelative("objectConcerned");
            SerializedProperty eventsToLaunchTypeRef = interactionListRef.FindPropertyRelative("eventsToLaunch");
            AddPopup(i, ref objectTypeRef, "Object concerned : ", typeof(ObjectSettings.ObjectType));

            if (!eventsToLaunchList.ContainsKey(i))
            {
                eventsToLaunchList.Add(i, new ReorderableList(serializedObject, eventsToLaunchTypeRef, true, true, true, true));

                ReorderableList list = eventsToLaunchList[i];
                list.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Events to launch");
                };

                list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                            element, GUIContent.none);
                };
            }

            eventsToLaunchList[i].DoLayoutList();

            if (toDelete.ContainsKey(i) && toDelete[i])
            {
                interactionProp.DeleteArrayElementAtIndex(i);
                toDelete[i] = false;
            }

        }


        serializedObject.ApplyModifiedProperties();
    }

    void AddPopup(int i, ref SerializedProperty ourSerializedProperty, string nameOfLabel, System.Type typeOfEnum)
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

        if (GUILayout.Button("X", miniButtonWidth))
        {
            if (!toDelete.ContainsKey(i))
            {
                toDelete.Add(i, true);
            }
            else
            {
                toDelete[i] = true;
            }
        }

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }

    #region STYLE_PROPERTIES
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
    #endregion
}

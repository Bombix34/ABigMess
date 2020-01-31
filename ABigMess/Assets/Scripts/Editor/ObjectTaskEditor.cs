using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ObjectTask))]
public class ObjectTaskEditor : Editor
{
    ObjectTask objectTask;
    SerializedObject GetTarget;

    SerializedProperty taskNameProp;
    SerializedProperty eventTypeProp;
    SerializedProperty numberDesiredProp;
    SerializedProperty countProp;
    SerializedProperty stateConcernedProp;
    SerializedProperty objectTypeConcernedProp;
    SerializedProperty destinationForBringProp;
    SerializedProperty destinationSpriteProp;
    SerializedProperty showCounterUIProp;

    private void OnEnable()
    {
        objectTask = (ObjectTask)target;
        GetTarget = new SerializedObject(objectTask);
        taskNameProp = GetTarget.FindProperty("taskName");
        eventTypeProp = GetTarget.FindProperty("eventType");
        numberDesiredProp = GetTarget.FindProperty("numberDesired");
        countProp = GetTarget.FindProperty("count");
        stateConcernedProp = GetTarget.FindProperty("stateConcerned");
        objectTypeConcernedProp = GetTarget.FindProperty("objectTypeConcerned");
        destinationForBringProp = GetTarget.FindProperty("destinationForBring");
        destinationSpriteProp = GetTarget.FindProperty("destinationSprite");
        showCounterUIProp = GetTarget.FindProperty("showCounterUI");
    }

    public override void OnInspectorGUI()
    {
        GetTarget.Update();

        taskNameProp.stringValue = EditorGUILayout.TextField("Task name :", taskNameProp.stringValue );

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        AddPopup(ref eventTypeProp, "Event type", typeof(ObjectTask.EventKeyWord));
        EditorGUILayout.Space();
        AddPopup(ref numberDesiredProp, "Number desired", typeof(ObjectTask.NumberType));
        if(numberDesiredProp.intValue==(int)ObjectTask.NumberType.number)
        {
            countProp.intValue = EditorGUILayout.IntField("Count :", countProp.intValue);
        }
        showCounterUIProp.boolValue=EditorGUILayout.Toggle("Show counter in UI : ",showCounterUIProp.boolValue);
        EditorGUILayout.Space();
        AddPopup(ref stateConcernedProp, "State of object :", typeof(ObjectState.State));
        AddPopup(ref objectTypeConcernedProp, "Objects concerned :", typeof(ObjectSettings.ObjectType));
        EditorGUILayout.Space();
        if (eventTypeProp.intValue == (int)ObjectTask.EventKeyWord.bring)
        {
            EditorGUILayout.LabelField("Warning : make sure there is area zone of type "+objectTask.destinationForBring +" in scene");
            AddPopup(ref destinationForBringProp, "Destination :", typeof(ObjectZoneArea.ZoneAreaType));
            EditorGUILayout.ObjectField(destinationSpriteProp);
        }
        GetTarget.ApplyModifiedProperties();
    }

    private void AddPopup(ref SerializedProperty ourSerializedProperty, string nameOfLabel, System.Type typeOfEnum)
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

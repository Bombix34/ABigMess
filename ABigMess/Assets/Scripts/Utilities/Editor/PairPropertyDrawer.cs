using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(BoolPair))]
public class PairPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.LabelField(position, "Should change");

        var shouldChangeRect = new Rect(position.x + 100, position.y, 30, position.height);
        var marksRect = new Rect(position.x + 120, position.y, 60, position.height);
        var valueRect = new Rect(position.x + 170, position.y, 30, position.height);

        EditorGUI.PropertyField(shouldChangeRect, property.FindPropertyRelative("Key"), GUIContent.none);

        EditorGUI.LabelField(marksRect, " Value:");

        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}

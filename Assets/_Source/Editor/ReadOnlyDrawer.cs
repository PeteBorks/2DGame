/**
 * ReadOnlyDrawer.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 05/06/18 (dd/mm//yy)
 * Reference from It3ration: https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
 */

using UnityEditor;
using UnityEngine;

// Don't forget to put this file inside an Editor folder!
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }

    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        GUI.enabled = false;
        EditorGUI.PropertyField(rect, prop, label, true);
        GUI.enabled = wasEnabled;
    }
}
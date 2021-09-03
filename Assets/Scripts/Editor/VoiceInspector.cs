using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Voice))]
public class VoiceInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty voice = property.FindPropertyRelative("voice");

        position.height = EditorGUIUtility.singleLineHeight; position.width -= 30;
        EditorGUI.LabelField(position, voice.objectReferenceValue.name);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        EditorGUI.indentLevel = 0;
        
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("voice"), GUIContent.none);
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 30f;
    }
}

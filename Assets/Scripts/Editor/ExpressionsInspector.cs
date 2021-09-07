using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Expression))]
public class ExpressionsInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty expression = null;

        try
        {
            expression = property.FindPropertyRelative("texture");

            position.height = EditorGUIUtility.singleLineHeight; position.width -= 80;
            EditorGUI.LabelField(position, expression.objectReferenceValue.name);
        } catch
        {
            //Ignore
        }

        position.x = position.xMax; position.width = 80; position.height = 80;
        expression.objectReferenceValue = EditorGUI.ObjectField(position, GUIContent.none, expression.objectReferenceValue, typeof(Texture2D), true);

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 80f;
    }
}

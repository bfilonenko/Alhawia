using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SingleLayerAttribute))]
public class SingleLayerAttributeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.LayerField(position, label, property.intValue);
    }
}

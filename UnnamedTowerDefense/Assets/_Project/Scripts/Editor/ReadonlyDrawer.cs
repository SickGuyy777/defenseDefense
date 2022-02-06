using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Attributes
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Store previous state
            var previousGUIState = GUI.enabled;
            
            // Disable GUI
            GUI.enabled = false;
            
            // Serialize
            EditorGUI.PropertyField(position, property, label);

            // Return to previous state
            GUI.enabled = previousGUIState;
        }
    }
}
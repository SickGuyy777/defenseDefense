using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class ReadonlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            base.OnGUI(position, property, label);
        }
    }
}
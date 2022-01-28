using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Path_System
{
    [CustomEditor(typeof(Path))]
    public class PathEditor : Editor
    {
        private Path path;
        private SerializedProperty pathFileProperty;
        
        private void OnEnable()
        {
            // Getting the PathFile reference
            path = (Path)target;
            pathFileProperty = serializedObject.FindProperty("reference");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField(pathFileProperty);
            var pathFile = (PathFile)pathFileProperty.objectReferenceValue;
            
            path.reference = pathFile;

            if (!pathFile) return;
            
            if (GUILayout.Button("Import From Reference"))
                path.UpdatePoints();
        }
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Path path = (Path)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("New Path Point"))
        {
            PathPoint point = path.NewPoint(path.transform.position);
            Selection.activeGameObject = point.gameObject;
        }
    }
}

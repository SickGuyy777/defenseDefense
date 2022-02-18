using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace _Project.Scripts.Grid
{
    [CustomEditor(typeof(GameGrid))]
    public class GameGridEditor : Editor
    {
        private GameGrid grid;
        
        private const string switchToEditBTN = "To Edit Mode";
        private const string switchToViewMode = "To View Mode";

        private void OnEnable() =>
                grid = serializedObject.targetObject.GetComponent<GameGrid>();

        private bool switched;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                grid.inEditMode = false;
                return;
            } 
            if (!switched)
            {
                grid.inEditMode = true;
                switched = true;
            }

            string editModeText = grid.inEditMode ? switchToViewMode : switchToEditBTN;

            bool switchMode = GUILayout.Button(editModeText);
            if (switchMode)
                grid.inEditMode = !grid.inEditMode;
        }
    }
}
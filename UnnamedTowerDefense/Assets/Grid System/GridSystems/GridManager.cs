using System;
using Grid_System.GridSystems.PathGridSystem;
using UnityEngine;

namespace Grid_System.GridSystems
{
    [RequireComponent(typeof(HoverableGrid))]
    [RequireComponent(typeof(PathGrid))]
    public class GridManager : MonoBehaviour
    {
        private HoverableGrid hoverableGrid;
        private PathGrid pathGrid;

        private void Awake()
        {
            hoverableGrid = GetComponent<HoverableGrid>();
            pathGrid = GetComponent<PathGrid>();
        }

        private void Update()
        {
            HoveredGridUpdate();
            PathGridUpdate();
        }

        private void HoveredGridUpdate()
        {
            if (!hoverableGrid.HasSelectedCell || !Input.GetKeyDown(KeyCode.R)) return;
            hoverableGrid.RotateCell();
        }

        private void PathGridUpdate()
        {
            if (!Input.GetMouseButtonDown(0) || !hoverableGrid.HasSelectedCell) return;

            PathCell selectedPath = pathGrid[hoverableGrid.LastSelectedPosition];
            if (selectedPath.IsPlaced)
                selectedPath.Remove();
            else
                selectedPath.Place(pathGrid.Placeable);
        }
    }
}
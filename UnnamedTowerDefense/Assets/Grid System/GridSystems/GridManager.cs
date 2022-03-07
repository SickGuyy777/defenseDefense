using System;
using UnityEngine;

namespace Grid_System.GridSystems
{
    [RequireComponent(typeof(HoverableGrid))]
    public class GridManager : MonoBehaviour
    {
        private HoverableGrid hoverableGrid;

        private void Awake()
        {
            hoverableGrid = GetComponent<HoverableGrid>();
        }

        private void Update() => UpdateHoveredCell();

        private void UpdateHoveredCell()
        {
            if (hoverableGrid.LastSelectedPosition.x < 0 || !Input.GetKeyDown(KeyCode.R)) return;
            hoverableGrid.RotateCell();
        }
    }
}
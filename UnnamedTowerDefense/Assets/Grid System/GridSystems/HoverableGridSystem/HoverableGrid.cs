using UnityEngine;

namespace Grid_System
{
    public class HoverableGrid : Grid<HoverableCell, HoverableCellObject, HoverableCellProperties, HoverableGrid>
    {
        public Vector2 CellScale { get; private set; }

        private void Awake()
        {
            UpdateGridProperties();
            hoverableCellProperties = new HoverableCellProperties();
            
            CellScale = new Vector2(HorizontalSpacing, VerticalSpacing);
        }


        private HoverableCellProperties hoverableCellProperties;
        public Vector2Int LastSelectedPosition { get; private set; } = -Vector2Int.one;

        private void Update()
        {
            if (Camera.main == null) return;
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HoverableCell selected = FromWorldPosition(mousePosition);
            if (selected == null || LastSelectedPosition == selected.GridPosition)
            {
                if (selected != null || LastSelectedPosition.x < 0) return;

                Cells[LastSelectedPosition.x, LastSelectedPosition.y].StopHover();
                LastSelectedPosition = -Vector2Int.one;
                return;
            }
            
            // Basically is the lastSelectedPosition an actual cell
            if (LastSelectedPosition.x >= 0)
                Cells[LastSelectedPosition.x, LastSelectedPosition.y].StopHover();

            selected.Properties = hoverableCellProperties;
            LastSelectedPosition = selected.GridPosition;
            selected.Hover();
        }

        public void RotateCell()
        {
            hoverableCellProperties.Rotate();

            if (LastSelectedPosition.x >= 0)
                Cells[LastSelectedPosition.x, LastSelectedPosition.y].Properties = hoverableCellProperties;
        }

        public void Hover(Vector2Int cellIndex) => Hover(cellIndex.x, cellIndex.y);
        public void Hover(int x, int y)
        {
            CheckCellPosition(x, y);
            Cells[x, y].Hover();
        }
        
        public void StopHover(Vector2Int cellIndex) => StopHover(cellIndex.x, cellIndex.y);
        public void StopHover(int x, int y)
        {
            CheckCellPosition(x, y);
            Cells[x, y].StopHover();
        }
    }
}
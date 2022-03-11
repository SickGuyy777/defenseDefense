using UnityEngine;

namespace Grid_System
{
    public class HoverableGrid : Grid<HoverableCell, HoverableCellObject, HoverableCellProperties, HoverableGrid>
    {
        public Vector2 CellScale { get; private set; }
        
        public CellEvent OnRotate;
        public CellEvent OnHover;
        public CellEvent OnStopHover;

        private void Awake()
        {
            UpdateGridProperties();
            HoverableCellProperties = new HoverableCellProperties();

            CellScale = new Vector2(HorizontalSpacing, VerticalSpacing);
        }

        protected override void OnInitialize() => InitializeCells();

        private void InitializeCells()
        {
            foreach (HoverableCell cell in Cells)
            {
                cell.Properties = HoverableCellProperties;
                
                cell.OnRotate += (_, _) => OnRotate?.Invoke(cell);
                cell.OnHover += () => OnHover?.Invoke(cell);
                cell.OnStopHover += () => OnStopHover?.Invoke(cell);
            }
        }


        public HoverableCellProperties HoverableCellProperties { get; private set; }
        public Vector2Int LastSelectedPosition { get; private set; } = -Vector2Int.one;

        public bool HasSelectedCell => LastSelectedPosition.x >= 0;

        public HoverableCell LastSelectedCell => HasSelectedCell ? this[LastSelectedPosition] : null;

        private void Update()
        {
            if (Camera.main == null) return;
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HoverableCell selected = FromWorldPosition(mousePosition);
            if (selected == null || LastSelectedPosition == selected.GridPosition)
            {
                if (selected != null || !HasSelectedCell) return;

                this[LastSelectedPosition].StopHover();
                LastSelectedPosition = -Vector2Int.one;
                return;
            }
            
            if (HasSelectedCell) this[LastSelectedPosition].StopHover();

            selected.Properties = HoverableCellProperties;
            LastSelectedPosition = selected.GridPosition;
            selected.Hover();
        }

        public void RotateCell()
        {
            HoverableCellProperties.Rotate();

            if (LastSelectedPosition.x >= 0)
                this[LastSelectedPosition].Properties = HoverableCellProperties;
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
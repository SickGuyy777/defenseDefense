using System;
using UnityEngine;

namespace Grid_System
{
    public class Grid<TCell, TCellObject, TCellProperties, TThis> : MonoBehaviour
        where TCell : Cell<TThis, TCellObject, TCellProperties, TCell>
        where TCellObject : CellObject<TCell, TThis, TCellProperties, TCellObject>
        where TCellProperties : CellProperties<TCell, TThis, TCellObject, TCellProperties>
        where TThis : Grid<TCell, TCellObject, TCellProperties, TThis>
    {
        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (value is < 1 or > 10) return;
                _width = value;
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (value is < 1 or > 10) return;
                _height = value;
            }
        }

        public float HorizontalSpacing { get; protected set; }
        public float VerticalSpacing { get; protected set; }

        public float XOffset { get; protected set; }
        public float YOffset { get; protected set; }

        public TCell[,] Cells { get; protected set; }


        public delegate void CellEvent(TCell cell);
        
        protected virtual void OnInitialize() { }

        public void Initialize()
        {
            InitializeGrid();
            OnInitialize();
        }

        protected void InitializeGrid()
        {
            UpdateGridProperties();
            Cells = new TCell[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Cells[i, j] = InstantiateCell(new Vector2Int(i, j));
                }
            }
        }

        protected virtual TCell InstantiateCell(Vector2Int gridPosition) =>
            (TCell) Activator.CreateInstance(typeof(TCell), this, gridPosition);

        protected virtual void UpdateGridProperties()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Could not find main camera!", this);
                return;
            }

            // cam.orthographicSize returns half of the Height
            float realHeight = cam.orthographicSize * 2;

            // Multiply the cam.aspect (Width/Height) by the Height to get the Width
            float realWidth = cam.aspect * realHeight;

            // Update spaces
            float horizontalSpacing = realWidth / Width;
            float verticalSpacing = realHeight / Height;

            // Update offsets
            float xOffset = Width % 2 == 0 ? horizontalSpacing / 2 : 0;
            float yOffset = Height % 2 == 0 ? verticalSpacing / 2 : 0;

            HorizontalSpacing = horizontalSpacing;
            VerticalSpacing = verticalSpacing;

            XOffset = xOffset;
            YOffset = yOffset;
        }


        // Indexer for cells
        public TCell this[Vector2Int gridPosition] => this[gridPosition.x, gridPosition.y];

        public TCell this[int i, int j]
        {
            get
            {
                CheckCellPosition(i, j);
                return Cells[i, j];
            }
        }


        // Get a cell's position in world position
        public virtual Vector2 ToWorldPosition(Vector2Int gridPosition)
        {
            float x = (gridPosition.x - Width / 2) * HorizontalSpacing + XOffset;
            float y = (gridPosition.y - Height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }

        // Get the closest grid position to the given position
        protected virtual TCell FromWorldPosition(Vector2 position)
        {
            Vector2 distance = position - (Vector2) transform.position;

            // Position is out of bounds
            if (Mathf.Abs(distance.x) > HorizontalSpacing * Width / 2 ||
                Mathf.Abs(distance.y) > VerticalSpacing * Height / 2)
                return null;

            float horizontalBound = Width / 2f;
            float verticalBound = Height / 2f;

            float x = position.x / HorizontalSpacing;
            float y = position.y / VerticalSpacing;

            var gridX = (int) (x + horizontalBound);
            var gridY = (int) (y + verticalBound);

            // Validate the position
            bool inBound = gridX < Width && gridX >= 0 && gridY < Height && gridY >= 0;

            return inBound ? Cells[gridX, gridY] : null;
        }


        protected bool CheckCellPosition(Vector2Int position) => CheckCellPosition(position.x, position.y);

        protected bool CheckCellPosition(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new IndexOutOfRangeException("Position is out of range.");

            return true;
        }
    }
}
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
        [SerializeField] [Range(1, 10)] protected int width;
        public int Width => width;

        [SerializeField] [Range(1, 10)] protected int height;
        public int Height => height;

        public float HorizontalSpacing { get; protected set; }
        public float VerticalSpacing { get; protected set; }
        
        public float XOffset { get; protected set; }
        public float YOffset { get; protected set; }
        
        public TCell[,] Cells { get; protected set; }
    
    
        protected virtual void OnStart() {}

    
        private void Start()
        {
            InitializeGrid();
            OnStart();
        }

        protected void InitializeGrid()
        {
            UpdateGridProperties();
            Cells = new TCell[width, height];

            TCell InstantiateCell(Vector2Int gridPosition) =>
                (TCell)Activator.CreateInstance(typeof(TCell), this, gridPosition);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cells[i, j] = InstantiateCell(new Vector2Int(i, j));
                }
            }
        }

        protected virtual void UpdateGridProperties()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Could not find main camera!", this);
                return;
            }

            // cam.orthographicSize returns half of the height
            float realHeight = cam.orthographicSize * 2;
            
            // Multiply the cam.aspect (width/height) by the height to get the width
            float realWidth = cam.aspect * realHeight;

            // Update spaces
            float horizontalSpacing = realWidth / width;
            float verticalSpacing = realHeight / height;

            // Update offsets
            float xOffset = width % 2 == 0 ? horizontalSpacing / 2 : 0;
            float yOffset = height % 2 == 0 ? verticalSpacing / 2 : 0;

            HorizontalSpacing = horizontalSpacing;
            VerticalSpacing = verticalSpacing;

            XOffset = xOffset;
            YOffset = yOffset;
        }

    
        // Indexer for cells
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
            float x = (gridPosition.x - width / 2) * HorizontalSpacing + XOffset;
            float y = (gridPosition.y - height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }
        
        // Get the closest grid position to the given position
        protected virtual TCell FromWorldPosition(Vector2 position)
        {
            Vector2 distance = position - (Vector2)transform.position;
            
            // Position is out of bounds
            if (Mathf.Abs(distance.x) > HorizontalSpacing * width / 2 ||
                Mathf.Abs(distance.y) > VerticalSpacing * height / 2)
                return null;

            float horizontalBound = width / 2f;
            float verticalBound = height / 2f;

            float x = position.x / HorizontalSpacing;
            float y = position.y / VerticalSpacing;
            
            var gridX = (int)(x + horizontalBound);
            var gridY = (int)(y + verticalBound);

            // Validate the position
            bool inBound = gridX < width && gridX >= 0 && gridY < height && gridY >= 0;

            return inBound ? Cells[gridX, gridY] : null;
        }


        protected bool CheckCellPosition(Vector2Int position) => CheckCellPosition(position.x, position.y);
        protected bool CheckCellPosition(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                throw new IndexOutOfRangeException("Position is out of range.");

            return true;
        }
    }
}
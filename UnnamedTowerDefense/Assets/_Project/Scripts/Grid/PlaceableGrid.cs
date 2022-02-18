using _Project.Scripts.Grid.Placeholders;
using Unity.VisualScripting;
using UnityEngine;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
using Exception = System.Exception;

namespace _Project.Scripts.Grid
{
    public class PlaceableGrid : Grid<PlaceableCell, PlaceableCellPlaceholder, PlaceableGrid>
    {
        // Initialize the grid
        protected override void OnStart() => Initialize();
        
        private void Initialize()
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Cannot initialize grid - invalid dimensions.",
                    new Exception());

            UpdateSpacingAndOffset();
            Cells = new PlaceableCell[width, height];
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Cells[i, j] = new PlaceableCell(this, new Vector2Int(i, j));
                }
            }
        }
        
        public override Vector2 FromGridPosition(Vector2Int gridPosition)
        {
            float x = (gridPosition.x - width / 2) * HorizontalSpacing + XOffset;
            float y = (gridPosition.y - height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }

        public void Place(Vector2Int gridPos, Placeable<PlaceableCell> template)
        {
            if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= width || gridPos.y >= height)
                throw new ArgumentOutOfRangeException("X or Y position is out of range.", new Exception());

            var ps = Cells[gridPos.x, gridPos.y].Placeholder.AddComponent<PlaceableSprite>();
            ps.placeableSprite = template.placeableSprite;
            
            Cells[gridPos.x, gridPos.y].Place(ps);
        }

        public void Release(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= width || gridPos.y >= height)
                throw new ArgumentOutOfRangeException("X or Y position is out of range.", new Exception());
            
            Cells[gridPos.x, gridPos.y].Release();
        }
    }
}
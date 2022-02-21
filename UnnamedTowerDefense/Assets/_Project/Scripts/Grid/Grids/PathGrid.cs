using System;
using _Project.Scripts.Grid.Placeholders;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class PathGrid : Grid<PathCell, PathPlaceholder, PathGrid>
    {
        // Initialize the grid
        protected override void OnStart() => Initialize();
        
        private void Initialize()
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Cannot initialize grid - invalid dimensions.",
                    new Exception());

            UpdateSpacingAndOffset();
            Cells = new PathCell[width, height];
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Cells[i, j] = new PathCell(this, new Vector2Int(i, j));
                }
            }
        }
        
        public override Vector2 FromGridPosition(Vector2Int gridPosition)
        {
            float x = (gridPosition.x - width / 2) * HorizontalSpacing + XOffset;
            float y = (gridPosition.y - height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }

        public void Place<T>(Vector2Int gridPos, T template)
            where T : Placeable<PathCell, PathGrid, PathPlaceholder>
        {
            if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= width || gridPos.y >= height)
                throw new ArgumentOutOfRangeException("X or Y position is out of range.", new Exception());

            var ps = Cells[gridPos.x, gridPos.y].Placeholder
                .AddComponent<T>();
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
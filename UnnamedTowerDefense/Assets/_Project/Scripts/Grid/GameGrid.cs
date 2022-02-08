using System;
using _Project.Scripts.Cursor;
using UnityEngine;
using UnityEngine.Assertions;
using _Project.Scripts.Grid.Hover_System;

namespace _Project.Scripts.Grid
{
    [AddComponentMenu("Grid/Game Grid")]
    public class GameGrid : Grid<WorldCell>
    {
        // Initialize the grid
        protected override void OnStart()
        {
            Initialize();
        }

        /// <summary>
        /// Get <paramref name="gridPosition"/> as a world position.
        /// </summary>
        /// <param name="gridPosition">The given position on the grid.</param>
        /// <returns>The world position of <paramref name="gridPosition"/>.</returns>
        public override Vector2 FromGridPosition(Vector2Int gridPosition)
        {
            var x = (gridPosition.x - width / 2) * HorizontalSpacing + XOffset;
            var y = (gridPosition.y - height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Initialize the cells in the grid
        /// </summary>
        private void Initialize()
        {
            // Assertions
            Assert.IsFalse(width <= 0 || height <= 0, "Cannot initialize grid - invalid dimensions.");

            UpdateSpacingAndOffset();
            Cells = new WorldCell[width, height];
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cells[i, j] = new WorldCell(this, new Vector2Int(i, j));
                }
            }
        }

        /// <summary>
        /// Update <see cref="Grid{T}.HorizontalSpacing"/> and <see cref="Grid{T}.VerticalSpacing"/>.
        /// </summary>
        private void UpdateSpacingAndOffset()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Could not find main camera!", this);
                return;
            }

            // cam.orthographicSize returns half of the height
            var realHeight = cam.orthographicSize * 2;
            
            // Multiply the cam.aspect (width/height) by the height to get the width
            var realWidth = cam.aspect * realHeight;

            // Update spaces
            HorizontalSpacing = realWidth / width;
            VerticalSpacing = realHeight / height;

            // Update offsets
            XOffset = width % 2 == 0 ? HorizontalSpacing / 2 : 0;
            YOffset = height % 2 == 0 ? VerticalSpacing / 2 : 0;
        }

        private WorldCell lastHovered;
        private void Update()
        {
            var selected = GetClosest(Mouse.Position);

            // Mouse is probably out of bounds
            if (selected == null)
                lastHovered?.Placeholder.SetAlpha(0);
            else
            {
                var changedHovered = selected.WorldPosition != lastHovered?.WorldPosition;
                
                if (changedHovered)
                    lastHovered?.Placeholder.SetAlpha(0);
                
                selected.Placeholder.SetAlpha(0.25f);
            }

            lastHovered = selected;
        }

        private WorldCell GetClosest(Vector2 position)
        {
            var distance = position - (Vector2)transform.position;
            
            // Position is out of bounds
            if (Math.Abs(distance.x) > HorizontalSpacing * width / 2 ||
                Math.Abs(distance.y) > VerticalSpacing * height / 2)
                return null;

            var horizontalBound = width / 2f;
            var verticalBound = height / 2f;

            var x = position.x / HorizontalSpacing;
            var y = position.y / VerticalSpacing;
            
            var gridX = (int)(x + horizontalBound);
            var gridY = (int)(y + verticalBound);
            
            // Validate the position
            var inBound = gridX < width && gridX >= 0 && gridY < height && gridY >= 0;
            
            return inBound ? Cells[gridX, gridY] : null;
        }
    }
}
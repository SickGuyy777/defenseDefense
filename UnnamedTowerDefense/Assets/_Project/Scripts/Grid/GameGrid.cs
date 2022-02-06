using _Project.Scripts.Attributes;
using _Project.Scripts.Cursor;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Grid
{
    [AddComponentMenu("Grid/Game Grid")]
    public class GameGrid : Grid<WorldCell>
    {
        [Readonly]
        [SerializeField]
        private WorldCellPlaceholder hoveredCell;
        
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

            // TODO: If there are cells present, 'disable' them and clear the cells array to initialize a new set.

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

        private void Update() => UpdateHovering();
        
        /// <summary>
        /// Update the mouse data
        /// </summary>
        private void UpdateHovering()
        {
            var position = Mouse.Position;
            var closestCell = GetClosestCell(position);
            
            if (closestCell == null)
            {
                if (hoveredCell == null) return;

                hoveredCell.IsHovered = false;
                hoveredCell = null;

                return;
            }
            
            // Changed hovering
            if (hoveredCell != null && hoveredCell.Cell.GridPosition == closestCell.Cell.GridPosition) return;

            if (hoveredCell != null)
                hoveredCell.IsHovered = false;
                
            closestCell.IsHovered = true;
            hoveredCell = closestCell;
        }

        /// <summary>
        /// Get the closest <see cref="WorldCell"/> to <paramref name="position"/>.
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <param name="radius">The radius to check for a cell</param>
        /// <returns>The closest <see cref="WorldCell"/> to <paramref name="position"/>.</returns>
        private WorldCellPlaceholder GetClosestCell(Vector2 position, float radius = 0.25f)
        {
            Assert.IsTrue(Cells.GetLength(0) > 0 && Cells.GetLength(1) > 0, "No cells found.");
            
            // Shoot a circle around the position, and check which cells we collide with
            var results = Physics2D.OverlapCircleAll(position, radius);

            // Position is outside of grid 
            if (results.Length == 0)
                return null;
            
            // Hit only one cell
            if (results.Length == 1)
            {
                var placeholder = results[0].GetComponent<WorldCellPlaceholder>();
                
                // If the hit is a cell, return it, otherwise, return null
                return placeholder;
            }

            // Hit more than one cell, get the closest one of the hits
            return GetClosest();

            WorldCellPlaceholder GetClosest()
            {
                WorldCellPlaceholder result = null;
                var shortestDistance = float.MaxValue;

                foreach (var item in results)
                {
                    var placeholder = item.GetComponent<WorldCellPlaceholder>();
                    if (!placeholder) // The hit is not a cell
                        continue;
                    
                    Vector2 cellPos = placeholder.transform.position;
                    var distance = (position - cellPos).magnitude;
                
                    if (distance >= shortestDistance) continue;
                
                    shortestDistance = distance;
                    result = placeholder;
                }
                
                return result;
            }
        }
    }
}
using _Project.Scripts.Attributes;
using UnityEngine;

namespace _Project.Scripts.Grid.Placeholders
{
    public class PathPlaceholder : CellPlaceholder<PathCell, PathGrid, PathPlaceholder>
    {
        public new const string DefaultSpritePath = "Assets/_Project/Sprites/Grid/Cell/Cell Placeholder.png";

        [Readonly]
        private PathGrid grid;

        /// <summary>
        /// Initialize the placeholder with <paramref name="cell"/>.
        /// </summary>
        /// <param name="cell">The reference <see cref="HoverableCell"/>.</param>
        public new void Initialize(PathCell cell)
        {
            // Assert if the cell is null
            if (cell == null)
            {
                Debug.LogError("Cannot initialize placeholder with a null cell.", this);
                return;
            }

            // Set placeholder properties
            Cell = cell;
            grid = cell.Parent;

            // Set GamObject properties
            name = cell.GridPosition.ToString();
            transform.position = cell.WorldPosition;
            transform.localScale = new Vector2(grid.HorizontalSpacing, grid.VerticalSpacing);

            // Set as child of the grid
            transform.SetParent(cell.Parent.transform);
        }

        /// <summary>
        /// Create a new <see cref="HoverableCellPlaceholder"/>.
        /// </summary>
        /// <param name="cell">The <see cref="HoverableCell"/> reference to create a <see cref="HoverableCellPlaceholder"/>.</param>
        /// <returns>The created <see cref="HoverableCellPlaceholder"/></returns>
        public new static PathPlaceholder New(PathCell cell)
        {
            // Create a new object
            var go = new GameObject(string.Empty);
            
            // Deactivate the object so awake doesn't trigger
            go.SetActive(false);
            
            // Add the component
            var placeholder = go.AddComponent<PathPlaceholder>();
            
            // Initialize before awake
            placeholder.Initialize(cell);
            
            // Re-activate the GameObject to trigger awake
            go.SetActive(true);

            return placeholder;
        }
    }
}
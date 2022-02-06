using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Grid
{
    [System.Serializable]
    public class WorldCell : Cell
    {
        public readonly Grid<WorldCell> Parent;
        
        public readonly Vector2Int GridPosition;
        public readonly Vector2 WorldPosition;

        public readonly WorldCellPlaceholder Placeholder;

        /// <summary>
        /// This <see cref="WorldCell"/>'s <see cref="WorldCellPlaceholder"/>'s <see cref="SpriteRenderer"/>.
        /// </summary>
        public SpriteRenderer Renderer => Placeholder.Renderer;

        /// <summary>
        /// Initialize a new <see cref="WorldCell"/>.
        /// </summary>
        /// <param name="parent">The parent grid of the cell.</param>
        /// <param name="gridPosition">The position in the parent grid.</param>
        public WorldCell(Grid<WorldCell> parent, Vector2Int gridPosition)
        {
            Assert.IsNotNull(parent, "Cannot initialize cell with an empty parent.");
            
            Parent = parent;
            GridPosition = gridPosition;
            WorldPosition = parent.FromGridPosition(gridPosition);

            Placeholder = WorldCellPlaceholder.New(this);
        }
    }
}
using UnityEngine;
using UnityEngine.Assertions;
using _Project.Scripts.Grid.Hover_System;

namespace _Project.Scripts.Grid
{
    [System.Serializable]
    public class WorldCell : Cell, IHoverable
    {
        public readonly Grid<WorldCell> Parent;
        
        public readonly Vector2Int GridPosition;
        public readonly Vector2 WorldPosition;

        public readonly WorldCellPlaceholder Placeholder;
        private bool _isHovered;

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
            
            // Set placeholder color to black
            Renderer.color = new Color(0, 0, 0, 0f);
        }

        bool IHoverable.IsHovered
        {
            get => _isHovered;
            set => _isHovered = value;
        }
        
        void IHoverable.OnHover() => Placeholder.SetAlpha(0.25f);
        void IHoverable.OnStopHover() => Placeholder.SetAlpha(1);
    }
}
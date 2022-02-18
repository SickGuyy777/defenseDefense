using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    [System.Serializable]
    public class HoverableCell : Cell<GameGrid, HoverableCellPlaceholder, HoverableCell>
    {
        public SpriteRenderer Renderer => Placeholder.Renderer;

        // Constructor
        /// <summary>
        /// Initialize a new <see cref="HoverableCell"/>.
        /// </summary>
        /// <param name="parent">The parent grid of the cell.</param>
        /// <param name="gridPosition">The position in the parent grid.</param>
        public HoverableCell(GameGrid parent, Vector2Int gridPosition) : base(parent, gridPosition)
        {
            // Set placeholder color to black
            Renderer.color = new Color(0, 0, 0, 0f);
        }


        // Hovering system
        [CanBeNull] public event System.Action OnHover;
        [CanBeNull] public event System.Action OnStopHover;

        private bool _isHovered;
        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                if (_isHovered == value)
                    return;

                _isHovered = value;
                Placeholder.SetAlpha(value ? 0.5f : 0f);
                
                if (value)
                    OnHover?.Invoke();
                else
                    OnStopHover?.Invoke();
            }
        }

        public void Hover() => IsHovered = true;
        public void StopHover() => IsHovered = false;

        public void SetSprite(Sprite sprite) => Renderer.sprite = sprite;
        public void ResetSprite() => SetSprite(HoverableCellPlaceholder.DefaultSprite);
    }
}
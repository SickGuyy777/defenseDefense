using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class PlaceableCell : Cell<PlaceableGrid, PlaceableCellPlaceholder, PlaceableCell>
    {
        public SpriteRenderer Renderer => Placeholder.Renderer;

        private readonly Sprite startSprite;


        // Constructor
        /// <summary>
        /// Initialize a new <see cref="HoverableCell"/>.
        /// </summary>
        /// <param name="parent">The parent grid of the cell.</param>
        /// <param name="gridPosition">The position in the parent grid.</param>
        public PlaceableCell(PlaceableGrid parent, Vector2Int gridPosition) : base(parent, gridPosition)
        {
            // Set placeholder color to black
            Renderer.color = new Color(255, 255, 255, 0f);
            Renderer.sortingOrder = -1;
            
            startSprite = Renderer.sprite;
        }
        
        
        // Placing system
        public Placeable<PlaceableCell> Placed { get; private set; }
        public bool IsOccupied => Placed != null;
        
        public void Place(Placeable<PlaceableCell> placeable)
        {
            Release();

            if (placeable == null)
                return;

            placeable.Place(this);
            Placed = placeable;

            Placeholder.Renderer.sprite = Placed.PlaceableSprite;
        }
        
        public void Release()
        {
            if (Placed == null)
                return;

            Renderer.sprite = startSprite;

            Placed.Release();
            Placed = null;
        }
    }
}
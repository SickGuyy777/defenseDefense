using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class PathCell : Cell<PathGrid, PathPlaceholder, PathCell>
    {
        public SpriteRenderer Renderer => Placeholder.Renderer;
        private readonly Sprite startSprite;

        public RotateDirection CurrentDir { get; protected set; }

        public PathCell(PathGrid parent, Vector2Int gridPosition) : base(parent, gridPosition)
        {
            // Set placeholder color to black
            Renderer.color = new Color(255, 255, 255, 0f);
            Renderer.sortingOrder = -1;
            
            startSprite = Renderer.sprite;
            CurrentDir = RotateDirection.Horizontal;
        }

        public PathCell(PathGrid parent, Vector2Int gridPosition, PathPlaceholder placeholder) :
                base(parent, gridPosition, placeholder)
        {
            // Set placeholder color to black
            Renderer.color = new Color(255, 255, 255, 0f);
            Renderer.sortingOrder = -1;
            
            startSprite = Renderer.sprite;
            CurrentDir = RotateDirection.Horizontal;
        }

        public void SetPlaceholder(PathPlaceholder placeholder) => Placeholder = placeholder;
        public void SetGridPosition(Vector2Int gridPosition)
        {
            if (GridPosition == gridPosition)
                return;

            GridPosition = gridPosition;
        }
        
        // Placing system
        public Placeable<PathCell, PathGrid, PathPlaceholder> Placed { get; protected set; }
        public bool IsOccupied => Placed != null;
        
        public void Place(Placeable<PathCell, PathGrid, PathPlaceholder> placeable)
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

        public void Rotate()
        {
            CurrentDir = CurrentDir ==
                         RotateDirection.Horizontal
                ? RotateDirection.Vertical
                : RotateDirection.Horizontal;
            Rotate(CurrentDir);
        }

        public void Rotate(RotateDirection dir) => Placeholder.transform.localRotation = GetRotation(dir);
        private Quaternion GetRotation(RotateDirection dir)
        {
            float angle = (dir == RotateDirection.Horizontal) ? 0 : 90;

            // angle = Mathf.Deg2Rad * angle; 
            return Quaternion.Euler(Placeholder.transform.eulerAngles.x, Placeholder.transform.eulerAngles.y, angle);
        }

        public enum RotateDirection
        {
            Vertical, Horizontal
        }
    }
}
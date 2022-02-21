using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class PathCell : Cell<PathGrid, PathPlaceholder, PathCell>
    {
        public SpriteRenderer Renderer => Placeholder.Renderer;
        private readonly Sprite startSprite;
        

        public PathCell(PathGrid parent, Vector2Int gridPosition) : base(parent, gridPosition)
        {
            // Set placeholder color to black
            Renderer.color = new Color(255, 255, 255, 0f);
            Renderer.sortingOrder = -1;
            
            startSprite = Renderer.sprite;
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

        public void Rotate(RotateDirection dir) => Placeholder.transform.rotation = GetRotation(dir);

        private static Quaternion GetRotation(RotateDirection dir)
        {
            return Quaternion.Euler(Vector3.zero);
        }

        public enum RotateDirection
        {
            Right, Left
        }
    }
}
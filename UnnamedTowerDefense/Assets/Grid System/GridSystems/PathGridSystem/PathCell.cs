using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    public class PathCell : Cell<PathGrid, PathCellObject, PathCellProperties, PathCell>
    {
        public Sprite Placed { get; private set; }
        public bool IsPlaced => Placed != null;

        public PathCell(PathGrid parent, Vector2Int gridPosition) : base(parent, gridPosition) {}

        public delegate void PlaceEvent(Sprite sprite);
        public event PlaceEvent OnPlace;

        public delegate void RemoveEvent();
        public event RemoveEvent OnRemove;
        
        public void Place(Sprite sprite)
        {
            if (IsPlaced) return;
            Placed = sprite;
            
            OnPlace?.Invoke(sprite);
        }
        

        public void Remove()
        {
            if (!IsPlaced) return;

            Placed = null;
            OnRemove?.Invoke();
        }
    }
}
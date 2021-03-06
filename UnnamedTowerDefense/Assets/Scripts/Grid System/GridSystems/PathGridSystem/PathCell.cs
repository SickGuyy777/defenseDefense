using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    public class PathCell : Cell<PathGrid, PathCellObject, PathCellProperties, PathCell>, IRotatable
    {
        public PathPlaceable Placed { get; private set; }
        public bool IsPlaced => Placed != null;

        public PathCell(PathGrid parent, Vector2Int gridPosition) : base(parent, gridPosition) {}

        public delegate void PlaceEvent(PathPlaceable placeable);
        public event PlaceEvent OnPlace;

        public delegate void RemoveEvent();
        public event RemoveEvent OnRemove;

        protected override void OnChangeProperties(PathCellProperties oldProperties, PathCellProperties newProperties)
            => Rotate(newProperties.RotationState);

        protected override bool CheckProperties(PathCellProperties other) =>
            other.RotationState == RotationState;

        public void Place(PathPlaceable sprite)
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

        private int _rotationState = -1;
        public int RotationState
        {
            get => _rotationState;
            set
            {
                if (value == _rotationState) return;
                if (value is < 0 or > 3)
                    throw new System.IndexOutOfRangeException("Rotation state is out of range.");

                int oldRot = _rotationState;
                _rotationState = value;
                
                OnRotate?.Invoke(oldRot, value);
            }
        }
        public void Rotate(int state) => RotationState = state;

        public event IRotatable.RotateEvent OnRotate;

        public override void Destroy() => Object.Destroy(CellObject.gameObject);
    }
}
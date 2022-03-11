using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    public class PathCellProperties : CellProperties<PathCell, PathGrid, PathCellObject, PathCellProperties>
    {
        private int _rotationState;
        public int RotationState
        {
            get => _rotationState;
            set
            {
                if (value is < 0 or > 3)
                    throw new System.IndexOutOfRangeException($"Invalid rotation state {value}.");
                
                _rotationState = value;
            }
        }

        public PathCellProperties() => RotationState = 0;

        public override string ToString() => $"rs{RotationState}";
        public new static PathCellProperties FromString(string source)
        {
            var result = new PathCellProperties();

            if (source.Length == 3 && source[0] == 'r' && source[1] == 's' &&
                int.TryParse(source[2].ToString(), out int rotationState))
                result.RotationState = rotationState;

            return result;
        }

        public void Rotate()
        {
            int newState = RotationState + 1;
            if (newState > 3) newState = 0;

            RotationState = newState;
        }

        public void Rotate(int state) => RotationState = state;
    }
}
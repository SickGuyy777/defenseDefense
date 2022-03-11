namespace Grid_System
{
    public class HoverableCellProperties :
            CellProperties<HoverableCell, HoverableGrid, HoverableCellObject, HoverableCellProperties>
    {
        private int _rotationState;
        public int RotationState
        {
            get => _rotationState;
            set
            {
                if (value is < 0 or > 1)
                        throw new System.IndexOutOfRangeException($"Invalid rotation state {value}.");
                
                _rotationState = value;
            }
        }

        public HoverableCellProperties() => RotationState = 0;

        public void Rotate() => RotationState = RotationState == 1 ? 0 : 1;
        public void Rotate(int state) => RotationState = state;
    }
}
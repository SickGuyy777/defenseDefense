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
                if (value is < 0 or > 3)
                        throw new System.IndexOutOfRangeException($"Invalid rotation state {value}.");
                
                _rotationState = value;
            }
        }

        public HoverableCellProperties() => RotationState = 0;

        public void Rotate()
        {
            int newState = RotationState + 1;
            if (newState > 3) newState = 0;

            RotationState = newState;
        }
        
        public void Rotate(int state) => RotationState = state;
    }
}
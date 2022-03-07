namespace Grid_System.GridSystems
{
    public interface IRotatable
    {
        int RotationState { get; set; }
        void Rotate(int state);

        delegate void RotateEvent(int oldState, int newState);
        event RotateEvent OnRotate;
    }
}
namespace _Project.Scripts.Grid.Hover_System
{
    public interface IHoverable
    {
        public bool IsHovered { get; protected set; }

        public void Hover()
        {
            IsHovered = true;
            OnHover();
        }

        public void StopHover()
        {
            IsHovered = false;
            OnStopHover();
        }
        
        protected void OnHover();
        protected void OnStopHover();
    }
}
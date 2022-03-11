using System;
using Grid_System.GridSystems;
using JetBrains.Annotations;
using UnityEngine;

namespace Grid_System
{
    [Serializable]
    public class HoverableCell : Cell<HoverableGrid, HoverableCellObject,
            HoverableCellProperties, HoverableCell>, IRotatable
    {
        private bool _isHovered;
        public bool IsHovered
        {
            get => _isHovered;
            protected set
            {
                if (value == _isHovered) return;

                _isHovered = value;
                
                if (value)
                    OnHover?.Invoke();
                else
                    OnStopHover?.Invoke();
            }
        }
        
        public delegate void HoverEvent();
        public event HoverEvent OnHover;
        public event HoverEvent OnStopHover;

        public HoverableCell(HoverableGrid parent, Vector2Int gridPosition) : base(parent, gridPosition) {}

        public void Hover() => IsHovered = true;
        public void StopHover() => IsHovered = false;

        private int _rotationState = -1;
        public int RotationState
        {
            get => _rotationState;
            set
            {
                if (value == _rotationState) return;
                if (value is > 1 or < 0)
                    throw new IndexOutOfRangeException("Rotation state is out of range.");

                int oldRot = _rotationState;
                _rotationState = value;
                
                OnRotate?.Invoke(oldRot, value);
            }
        }

        public void Rotate() => Rotate(RotationState == 0 ? 1 : 0);

        protected override void OnChangeProperties(HoverableCellProperties oldProperties,
                HoverableCellProperties newProperties) =>
            Rotate(newProperties.RotationState);

        protected override bool CheckProperties([NotNull] HoverableCellProperties other) =>
            other.RotationState == RotationState;
        
        public void Rotate(int state) => RotationState = state;
        public event IRotatable.RotateEvent OnRotate;
    }
}
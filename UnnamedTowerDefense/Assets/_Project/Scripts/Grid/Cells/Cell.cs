using System.Reflection;
using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class Cell<TGrid, TPlaceHolder, TThis>
        where TGrid : Grid<TThis, TPlaceHolder, TGrid>
        where TThis : Cell<TGrid, TPlaceHolder, TThis>
        where TPlaceHolder : CellPlaceholder<TThis, TGrid, TPlaceHolder>
    {
        private Vector2Int _gridPosition;

        public Vector2Int GridPosition
        {
            get => _gridPosition;
            protected set
            {
                _gridPosition = value;
                WorldPosition = Parent.FromGridPosition(value);
            }
        }
        public Vector2 WorldPosition { get; protected set; }

        public readonly TGrid Parent;

        private TPlaceHolder _placeholder;

        public TPlaceHolder Placeholder
        {
            get => _placeholder;
            protected set
            {
                if (value != null && _placeholder == value)
                    return;

                _placeholder = value;
            }
        }

        protected Cell(TGrid parent, Vector2Int gridPosition)
        {
            Parent = parent;
            GridPosition = gridPosition;

            MethodInfo newMethod = typeof(TPlaceHolder).GetMethod("New");
            var placeHolder = (TPlaceHolder)newMethod?.Invoke(default, new object[] { this });
            
            Placeholder = placeHolder;
        }

        protected Cell(TGrid parent, Vector2Int gridPosition, TPlaceHolder placeholder)
        {
            Parent = parent;
            GridPosition = gridPosition;

            Placeholder = placeholder;
        }
    }
}
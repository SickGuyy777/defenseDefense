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
        public readonly Vector2Int GridPosition;
        public readonly Vector2 WorldPosition;

        public readonly TGrid Parent;
        public TPlaceHolder Placeholder { get; protected set; }

        public Cell(TGrid parent, Vector2Int gridPosition)
        {
            Parent = parent;
            GridPosition = gridPosition;
            WorldPosition = Parent.FromGridPosition(gridPosition);

            MethodInfo newMethod = typeof(TPlaceHolder).GetMethod("New");
            var placeHolder = (TPlaceHolder)newMethod?.Invoke(default, new object[] { this });
            
            Placeholder = placeHolder;
        }
    }
}
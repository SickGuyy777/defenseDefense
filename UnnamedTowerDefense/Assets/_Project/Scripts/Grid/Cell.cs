using System.Reflection;
using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public interface ICell { }
    
    public class Cell<T, TPlaceHolder, TThis> : ICell
        where T : IGrid
        where TThis : ICell
        where TPlaceHolder : IPlaceHolder
    {
        public readonly Vector2Int GridPosition;
        public readonly Vector2 WorldPosition;

        public readonly T Parent;
        public TPlaceHolder Placeholder { get; protected set; }

        public Cell(T parent, Vector2Int gridPosition)
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
using UnityEngine;

namespace Grid_System
{
    public class Cell<TGrid, TCellObject, TCellProperties, TThis>
        where TGrid : Grid<TThis, TCellObject, TCellProperties, TGrid>
        where TCellObject : CellObject<TThis, TGrid, TCellProperties, TCellObject>
        where TThis : Cell<TGrid, TCellObject, TCellProperties, TThis>
        where TCellProperties : CellProperties<TThis, TGrid, TCellObject, TCellProperties>
    {
        public TGrid Parent { get; protected set; }

        private Vector2Int _gridPosition;
        public Vector2Int GridPosition
        {
            get => _gridPosition;
            set
            {
                _gridPosition = value;
                WorldPosition = Parent.ToWorldPosition(_gridPosition);
            }
        }
        
        public Vector2 WorldPosition { get; private set; }
        public TCellObject CellObject { get; protected set; }
        
        private TCellProperties _properties;
        public TCellProperties Properties
        {
            get => _properties;
            set
            {
                if (_properties == null && value == null || CheckProperties(value)) return;

                OnChangeProperties(_properties, value);
                _properties = value;
            }
        }

        protected virtual bool CheckProperties(TCellProperties other) => Properties == other;

        protected virtual void OnChangeProperties(TCellProperties oldProperties, TCellProperties newProperties) {}

        // Construct a new cell
        public Cell(TGrid parent, Vector2Int gridPosition)
        {
            Parent = parent;
            GridPosition = gridPosition;
            CellObject = NewCellObject();
        }

        protected TCellObject NewCellObject()
        {
            var go = new GameObject($"({GridPosition.x}, {GridPosition.y})'s CellObject");
            go.SetActive(false);

            var cellObject = go.AddComponent<TCellObject>();
            cellObject.Initialize((TThis)this);
            
            go.SetActive(true);
            return cellObject;
        }
    }

    [System.Serializable]
    public class CellProperties<TCell, TGrid, TCellObject, TThis>
        where TCell : Cell<TGrid, TCellObject, TThis, TCell>
        where TGrid : Grid<TCell, TCellObject, TThis, TGrid>
        where TCellObject : CellObject<TCell, TGrid, TThis, TCellObject>
        where TThis : CellProperties<TCell, TGrid, TCellObject, TThis>  { }
}
using UnityEngine;

namespace Grid_System
{
    public class CellObject<TCell, TGrid, TCellProperties, TThis> : MonoBehaviour
        where TCell : Cell<TGrid, TThis, TCellProperties, TCell>
        where TGrid : Grid<TCell, TThis, TCellProperties, TGrid>
        where TCellProperties : CellProperties<TCell, TGrid, TThis, TCellProperties>
        where TThis : CellObject<TCell, TGrid, TCellProperties, TThis>
    {
        public TCell Cell { get; protected set; }

        public void Initialize(TCell cell)
        {
            Cell = cell;
            OnInitialize();
        }
        
        protected virtual void OnInitialize() { }
    }
}
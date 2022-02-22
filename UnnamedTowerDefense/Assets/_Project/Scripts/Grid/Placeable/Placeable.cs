using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class Placeable<TCell, TGrid, TPlaceholder> : MonoBehaviour
        where TCell : Cell<TGrid, TPlaceholder, TCell>
        where TGrid : Grid<TCell, TPlaceholder, TGrid>
        where TPlaceholder : CellPlaceholder<TCell, TGrid, TPlaceholder>
    {
        public SpriteRenderer Renderer { get; private set; }
        public TCell Cell { get; private set; }
        
        public Sprite placeableSprite;

        public Sprite PlaceableSprite => placeableSprite;
        
        protected virtual void OnAwake() { }
        protected virtual void OnPlace(TCell _) {}
        protected virtual void OnRelease() { }
        
        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();

            if (!Renderer)
                Renderer = gameObject.AddComponent<SpriteRenderer>();
            
            OnAwake();
        }

        public void Place(TCell cell)
        {
            Cell = cell;
            OnPlace(cell);
        }

        public void Release() => OnRelease();
    }
}
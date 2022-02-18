using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Grid
{
    public interface IPlaceable<TCell>
        where TCell : ICell
    {
        void Place(TCell cell);
    }
    
    public class Placeable<TCell> : MonoBehaviour, IPlaceable<TCell>
        where TCell : ICell
    {
        public SpriteRenderer Renderer { get; private set; }
        protected TCell Cell { get; private set; }
        
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
        
        public enum Type { Default, Path }
    }
}
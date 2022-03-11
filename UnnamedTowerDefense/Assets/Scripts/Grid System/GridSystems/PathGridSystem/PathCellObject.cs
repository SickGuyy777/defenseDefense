using System;
using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    public class PathCellObject : CellObject<PathCell, PathGrid, PathCellProperties, PathCellObject>
    {
        public SpriteRenderer Renderer { get; private set; }
        
        private void Awake() => Renderer = gameObject.AddComponent<SpriteRenderer>();

        protected override void OnInitialize()
        {
            transform.position = Cell.WorldPosition;
            transform.SetParent(Cell.Parent.transform);
            
            transform.localScale = Cell.Parent.CellScale;
            
            Cell.OnPlace += OnPlace;
            Cell.OnRemove += OnRemove;
        }

        private void OnPlace(PathPlaceable placeable) => Renderer.sprite = placeable;
        private void OnRemove() => Renderer.sprite = null;
    }
}
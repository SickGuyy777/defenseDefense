using System.Collections.Generic;
using _Project.Scripts.Cursor;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    [AddComponentMenu("Grid/Game Grid")]
    public class GameGrid : Grid<HoverableCell, HoverableCellPlaceholder, GameGrid>
    {
        // Grid properties
        [SerializeField]
        private List<PlaceableItem> prefabs;

        private int objIndex = -1;

        private PlaceableGrid placeableGrid;
        
        [HideInInspector]
        public bool inEditMode;

        
        // Initialize the grid
        protected override void OnStart()
        {
            placeableGrid = new GameObject("Placeable Grid", typeof(PlaceableGrid))
                    .GetComponent<PlaceableGrid>();
            
            placeableGrid.width = width;
            placeableGrid.height = height;
            
            Initialize();

            if (prefabs.Count == 0)
                throw new System.Exception("Found no prefabs!"); 

            objIndex = 0;
        }

        private void Initialize()
        {
            if (width <= 0 || height <= 0)
                throw new System.ArgumentOutOfRangeException("Cannot initialize grid - invalid dimensions.",
                    new System.Exception());

            UpdateSpacingAndOffset();
            Cells = new HoverableCell[width, height];
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Cells[i, j] = new HoverableCell(this, new Vector2Int(i, j));
                }
            }
        }

        public override Vector2 FromGridPosition(Vector2Int gridPosition)
        {
            float x = (gridPosition.x - width / 2) * HorizontalSpacing + XOffset;
            float y = (gridPosition.y - height / 2) * VerticalSpacing + YOffset;

            return new Vector2(x, y);
        }

        private void Update()
        {
            if (inEditMode)
                EditModeUpdate();
            else
                ViewModeUpdate();
        }

        private HoverableCell lastHoveredCell;
        public void EditModeUpdate()
        {
            bool switchPrefab = prefabs.Count > 0 && Input.GetKeyDown(KeyCode.H);
            if (switchPrefab)
                objIndex = objIndex == prefabs.Count - 1 ? 0 : objIndex + 1;

            HoverableCell lastHovered = lastHoveredCell;
            HoverableCell hovered = GetClosest(Mouse.Position);
            SelectCell(hovered);
            
            if (hovered == null)
                return;

            Vector2Int gridPos = hovered.GridPosition;
            var placed = placeableGrid.Cells[gridPos.x, gridPos.y].Placed;
            bool notSamePlaced = !placed || placed.PlaceableSprite != prefabs[objIndex].Prefab.PlaceableSprite;
            if (Input.GetMouseButtonDown(0))
            {
                if (notSamePlaced)
                    placeableGrid.Place(gridPos, prefabs[objIndex].Prefab);
                else
                    placeableGrid.Release(gridPos);
            }
            
            if (hovered != lastHovered || switchPrefab) return;
            
            if (notSamePlaced)
            {
                hovered.Placeholder.SetRed(1f);
                hovered.Placeholder.SetGreen(1f);
                hovered.Placeholder.SetBlue(1f);
                hovered.SetSprite(prefabs[objIndex].Prefab.PlaceableSprite);

                if (Input.GetMouseButtonDown(0))
                    placeableGrid.Place(gridPos, prefabs[objIndex].Prefab);
            }
            else
            {
                hovered.Placeholder.SetRed(0.5f);
                hovered.Placeholder.SetGreen(0f);
                hovered.Placeholder.SetBlue(0f);
                hovered.ResetSprite();
                
                if (Input.GetMouseButtonDown(0))
                    placeableGrid.Release(gridPos);
            }

            void SelectCell(HoverableCell selected)
            {
                if (selected == null)
                    lastHoveredCell?.StopHover();
                else
                {
                    bool changedHovered = selected.WorldPosition != lastHoveredCell?.WorldPosition;
                
                    if (changedHovered)
                        lastHoveredCell?.StopHover();
                    
                    selected.Hover();
                }

                lastHoveredCell = selected;
            }
        }

        public void ViewModeUpdate()
        {
            
        }

        
        #region GUI
        private void OnGUI()
        {
            if (inEditMode)
            {
                GUI.Label(new Rect(new Vector2(20, 20), new Vector2(150, 20)),
                    "Placing: " + prefabs[objIndex].Name);
            }
        }
        #endregion
        
        [System.Serializable]
        private class PlaceableItem
        {
            [SerializeField] private string name;
            [SerializeField] private PlaceableSprite prefab;
            
            public string Name => name;
            public PlaceableSprite Prefab => prefab;
        }
    }
}
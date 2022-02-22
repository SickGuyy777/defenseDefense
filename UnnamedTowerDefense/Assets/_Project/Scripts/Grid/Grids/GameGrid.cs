using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Cursor;
using _Project.Scripts.Grid.Placeholders;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    [AddComponentMenu("Grid/Game Grid")]
    public class GameGrid : Grid<HoverableCell, HoverableCellPlaceholder, GameGrid>
    {
        // Grid properties
        [SerializeField] private List<PlaceableSpriteItem> spritePrefabs;
        [SerializeField] private List<PathItem> pathPrefabs;
        
        private int objIndex = -1;
        private int pathIndex = -1;

        private PlaceableGrid placeableGrid;
        private PathGrid pathGrid;
        
        [HideInInspector]
        public bool inEditMode;

        private bool placingPath;

        
        // Initialize the grid
        protected override void OnStart()
        {
            placeableGrid = new GameObject("Placeable Grid", typeof(PlaceableGrid))
                    .GetComponent<PlaceableGrid>();
            
            placeableGrid.width = width;
            placeableGrid.height = height;

            pathGrid = new GameObject("Path Grid", typeof(PathGrid)).GetComponent<PathGrid>();

            pathGrid.width = width;
            pathGrid.height = height;

            Initialize();

            if (spritePrefabs.Count == 0)
                throw new System.Exception("Found no prefabs!");

            if (pathPrefabs.Count == 0)
                throw new System.Exception("Found no path prefabs!");
            
            objIndex = 0;
            pathIndex = 0;
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

            StartCoroutine(SetDraggablePath());
        }

        private IEnumerator SetDraggablePath()
        {
            yield return new WaitUntil(() => pathGrid.Cells != null);
            draggedPath = new PathCell(pathGrid, new Vector2Int(0, 0), pathGrid.Cells[0, 0].Placeholder);
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
        private PathCell draggedPath;
        
        public void EditModeUpdate()
        {
            bool switchPrefab = (placingPath ? pathPrefabs.Count > 0 : spritePrefabs.Count > 0) &&
                                Input.GetKeyDown(KeyCode.H);
            if (switchPrefab)
            {
                if (placingPath)
                    pathIndex = pathIndex == pathPrefabs.Count - 1 ? 0 : pathIndex + 1;
                else
                    objIndex = objIndex == spritePrefabs.Count - 1 ? 0 : objIndex + 1;
            }

            bool switchPathMode = Input.GetKeyDown(KeyCode.P);
            if (switchPathMode)
                placingPath = !placingPath;

            HoverableCell lastHovered = lastHoveredCell;
            HoverableCell hovered = GetClosest(Mouse.Position);
            SelectCell(hovered);

            if (switchPathMode)
            {
                hovered.StopHover();
                hovered.Hover();
            }
            
            if (hovered == null)
                return;

            Vector2Int gridPos = hovered.GridPosition;

            bool notSamePlaced;
            if (placingPath)
            {
                draggedPath.SetGridPosition(gridPos);

                var placed = pathGrid.Cells[gridPos.x, gridPos.y].Placed;
                notSamePlaced = !placed || placed.placeableSprite != pathPrefabs[pathIndex].Preafb.PlaceableSprite;

                PathPlaceholder pathPlaceholder = GetPathPlaceholder();

                draggedPath.SetPlaceholder(pathPlaceholder);
                pathPlaceholder.CopyFromCell(draggedPath);
                
                if (Input.GetKeyDown(KeyCode.R))
                    draggedPath.Rotate();

                if (Input.GetMouseButtonDown(0))
                {
                    if (notSamePlaced)
                    {
                        placeableGrid.Cells[gridPos.x, gridPos.y].Release();
                        pathGrid.Place(gridPos, pathPrefabs[pathIndex].Preafb); 
                        
                        pathGrid.Cells[gridPos.x, gridPos.y].Placeholder.CopyFromCell(draggedPath);
                    }
                    else
                        pathGrid.Release(gridPos);
                }

                PathPlaceholder GetPathPlaceholder()
                {
                    var result = hovered.Placeholder.GetComponent<PathPlaceholder>();

                    if (!result)
                        result = hovered.Placeholder.AddComponent<PathPlaceholder>();
                    return result;
                }
            }
            else
            {
                var placed = placeableGrid.Cells[gridPos.x, gridPos.y].Placed;
                notSamePlaced = !placed || placed.PlaceableSprite != spritePrefabs[objIndex].Prefab.PlaceableSprite;
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (notSamePlaced)
                    {
                        pathGrid.Cells[gridPos.x, gridPos.y].Release();
                        placeableGrid.Place(gridPos, spritePrefabs[objIndex].Prefab);
                    }
                    else
                        placeableGrid.Release(gridPos);
                }
                
                if (hovered != lastHovered || switchPrefab) return;
            }
            
            if (notSamePlaced)
            {
                hovered.Placeholder.SetRed(1f);
                hovered.Placeholder.SetGreen(1f);
                hovered.Placeholder.SetBlue(1f);

                Sprite hoverSprite = placingPath
                    ? pathPrefabs[pathIndex].Preafb.PlaceableSprite
                    : spritePrefabs[objIndex].Prefab.PlaceableSprite; 
                
                hovered.SetSprite(hoverSprite);
            }
            else
            {
                hovered.Placeholder.SetRed(0.5f);
                hovered.Placeholder.SetGreen(0f);
                hovered.Placeholder.SetBlue(0f);
                hovered.ResetSprite();
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
                    "Placing: " + spritePrefabs[objIndex].Name);
            }
            
            GUI.Label(new Rect(new Vector2(20, 60), new Vector2(150, 20)),
                $"{(placingPath ? string.Empty : "Not ")}Placing Path");
        }
        #endregion
        
        [System.Serializable]
        private class PlaceableSpriteItem
        {
            [SerializeField] private string name;
            [SerializeField] private PlaceableSprite prefab;
            
            public string Name => name;
            public PlaceableSprite Prefab => prefab;
        }

        [System.Serializable]
        private class PathItem
        {
            [SerializeField] private string name;
            [SerializeField] private PlaceablePath prefab;

            public string Name => name;
            public PlaceablePath Preafb => prefab;
        }
    }
}
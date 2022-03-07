using System;
using UnityEngine;

namespace Grid_System.GridSystems.PathGridSystem
{
    public class PathGrid : Grid<PathCell, PathCellObject, PathCellProperties, PathGrid>
    {
        [SerializeField] private Sprite placeableSprite;
        public Sprite Placeable => placeableSprite;

        public Vector2 CellScale { get; private set; }
        
        private void Awake()
        {
            UpdateGridProperties();
            CellScale = new Vector2(HorizontalSpacing, VerticalSpacing);
        }


        public void Place(Vector2Int gridPosition) => Place(gridPosition.x, gridPosition.y);
        public void Place(int xPos, int yPos)
        {
            CheckCellPosition(xPos, yPos);
            Cells[xPos, yPos].Place(Placeable);
        }

        public void Remove(Vector2Int gridPosition) => Remove(gridPosition.x, gridPosition.y);
        public void Remove(int xPos, int yPos)
        {
            CheckCellPosition(xPos, yPos);
            Cells[xPos, yPos].Remove();
        }
    }
}
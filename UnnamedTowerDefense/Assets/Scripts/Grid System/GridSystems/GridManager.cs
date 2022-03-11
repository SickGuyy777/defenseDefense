using Grid_System.GridSystems.PathGridSystem;
using UnityEngine;

namespace Grid_System.GridSystems
{
    [RequireComponent(typeof(HoverableGrid))]
    [RequireComponent(typeof(PathGrid))]
    [DefaultExecutionOrder(-int.MaxValue)]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] private int width;
        [SerializeField] [Range(1, 10)] private int height;
        
        private HoverableGrid hoverableGrid;
        private PathGrid pathGrid;

        [SerializeField] private Sprite pathSprite;

        private void Awake()
        {
            hoverableGrid = GetComponent<HoverableGrid>();
            pathGrid = GetComponent<PathGrid>();

            hoverableGrid.Width = width;
            hoverableGrid.Height = height;

            pathGrid.Width = width;
            pathGrid.Height = height;
        }

        private void Start()
        {
            hoverableGrid.Initialize();
            pathGrid.Initialize();
            
            HoverableGridInit();
            PathGridInit();
        }

        private void Update()
        {
            HoveredGridUpdate();
            PathGridUpdate();
        }

        private void HoverableGridInit()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    hoverableGrid[i, j].CellObject.Sprite = pathSprite;

                    // Reset color
                    hoverableGrid[i, j].CellObject.SetAlpha(0f);
                }
            }
            
            hoverableGrid.OnRotate += OnRotate;
            hoverableGrid.OnHover += OnHover;
            hoverableGrid.OnStopHover += OnStopHover;
        }

        private void PathGridInit() => pathGrid.OnRotate += PathOnRotate;

        private void PathOnRotate(PathCell cell)
        {
            Vector3 rot = cell.CellObject.transform.eulerAngles;
            cell.CellObject.transform.localRotation = Quaternion.Euler(rot.x, rot.y, cell.RotationState * 90);

            Vector2 newScale = pathGrid.CellScale;
            if (cell.RotationState == 1)
                newScale = new Vector2(newScale.y, newScale.x);

            cell.CellObject.transform.localScale = newScale;
        }

        private static void OnRotate(HoverableCell cell)
        {
            Vector3 rot = cell.CellObject.transform.eulerAngles;
            cell.CellObject.transform.localRotation = Quaternion.Euler(rot.x, rot.y, cell.RotationState * 90);
            
            Vector2 oldRot = cell.CellObject.transform.localScale;
            cell.CellObject.transform.localScale = new Vector2(oldRot.y, oldRot.x);
        }

        private static void OnHover(HoverableCell cell) => cell.CellObject.SetAlpha(0.5f);
        private static void OnStopHover(HoverableCell cell) => cell.CellObject.SetAlpha(0f);

        private void HoveredGridUpdate()
        {
            if (!hoverableGrid.HasSelectedCell || !Input.GetKeyDown(KeyCode.R)) return;
            hoverableGrid.RotateCell();
        }

        private void PathGridUpdate()
        {
            if (!Input.GetMouseButtonDown(0) || !hoverableGrid.HasSelectedCell) return;

            PathCell selectedPath = pathGrid[hoverableGrid.LastSelectedPosition];
            if (selectedPath.IsPlaced)
                selectedPath.Remove();
            else
            {
                selectedPath.Properties = GetPathProperties();
                selectedPath.Place(pathGrid.Placeable);
            }
        }

        private PathCellProperties GetPathProperties()
        {
            var newProperties = new PathCellProperties();
            newProperties.Rotate(hoverableGrid.HoverableCellProperties.RotationState);

            return newProperties;
        }
    }
}
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

        [SerializeField] private PathPlaceable[] placeables;
        [SerializeField] private PathSetup setup;

        private int _placeableIndex;
        public int PlaceableIndex
        {
            get => _placeableIndex;
            set
            {
                if (value == _placeableIndex) return;

                _placeableIndex = value;
                
                pathGrid.placeable = placeables[_placeableIndex];
                
                HoverableGridInit(false);
                hoverableGrid.ForceUpdate();
            }
        }
        
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
            HoverableGridInit();
            
            _placeableIndex = -1;
            PlaceableIndex = 0;
            
            // Load twice because loading rotation works only with that ¯\_(ツ)_/¯
            LoadPathSetup();
            LoadPathSetup();
        }

        private void Update()
        {
            HoveredGridUpdate();
            PathGridUpdate();

            if (Input.GetKeyDown(KeyCode.S)) SavePathSetup();
            else if (Input.GetKeyDown(KeyCode.L)) LoadPathSetup();
            else if (Input.GetKeyDown(KeyCode.F))
            {
                int newIndex = PlaceableIndex + 1;
                if (newIndex >= placeables.Length) newIndex = 0;

                PlaceableIndex = newIndex;
            }
        }

        private void HoverableGridInit(bool subscribeToEvents = true)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    hoverableGrid[i, j].CellObject.Sprite = placeables[PlaceableIndex];

                    // Reset color
                    hoverableGrid[i, j].CellObject.SetAlpha(0f);
                }
            }
            
            if (!subscribeToEvents) return;
            
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
            if (cell.RotationState is 1 or 3)
                newScale = new Vector2(newScale.y, newScale.x);

            cell.CellObject.transform.localScale = newScale;
        }

        private void OnRotate(HoverableCell cell)
        {
            int rotationSource = cell.RotationState;
            if (PlaceableIndex == 0 && cell.RotationState > 1) rotationSource -= 2;

            Vector3 rot = cell.CellObject.transform.eulerAngles;
            cell.CellObject.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rotationSource * 90);
            
            Vector2 newScale = hoverableGrid.CellScale;
            if (cell.RotationState is 1 or 3)
                newScale = new Vector2(newScale.y, newScale.x);

            cell.CellObject.transform.localScale = newScale;
        }

        private void OnHover(HoverableCell cell)
        {
            cell.CellObject.SetAlpha(0.5f);
            
            Vector2 newScale = hoverableGrid.CellScale;
            if (hoverableGrid.HoverableCellProperties.RotationState is 1 or 3)
                newScale = new Vector2(newScale.y, newScale.x);

            cell.CellObject.transform.localScale = newScale;
        }

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
                selectedPath.Place(pathGrid.placeable);
            }
        }

        private PathCellProperties GetPathProperties()
        {
            var newProperties = new PathCellProperties();
            newProperties.Rotate(hoverableGrid.HoverableCellProperties.RotationState);

            return newProperties;
        }


        public void LoadPathSetup()
        {
            setup.SetToGrid(ref pathGrid);
            PathGridInit();
        }

        public void SavePathSetup() => setup.FromGrid(pathGrid);
    }
}
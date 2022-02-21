using _Project.Scripts.Attributes;
using _Project.Scripts.Grid;
using _Project.Scripts.Grid.Placeholders;
using UnityEngine;

public class Grid<TCell, TPlaceHolder, TThis> : MonoBehaviour
    where TPlaceHolder : CellPlaceholder<TCell, TThis, TPlaceHolder>
    where TCell : Cell<TThis, TPlaceHolder, TCell>
    where TThis : Grid<TCell, TPlaceHolder, TThis>
{

    [Readonly]
    public int width;
    
    [Readonly]
    public int height;
    
    protected float horizontalSpacing;

    /// <summary>
    /// The width of one grid cell (in world units)
    /// </summary>
    public float HorizontalSpacing => horizontalSpacing;

    protected float verticalSpacing;

    /// <summary>
    /// The height of one grid cell (in world units)
    /// </summary>
    public float VerticalSpacing => verticalSpacing;

    /// <summary>
    /// The horizontal offset of each cell (in world units)
    /// </summary>
    public float XOffset { get; protected set; }
    /// <summary>
    /// The vertical offset of each cell (in world units)
    /// </summary>
    public float YOffset { get; protected set; }

    public virtual Vector2 FromGridPosition(Vector2Int gridPosition) => Vector2.zero;

    protected virtual void UpdateSpacingAndOffset()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Could not find main camera!", this);
            return;
        }

        // cam.orthographicSize returns half of the height
        float realHeight = cam.orthographicSize * 2;
            
        // Multiply the cam.aspect (width/height) by the height to get the width
        float realWidth = cam.aspect * realHeight;

        // Update spaces
        horizontalSpacing = realWidth / width;
        verticalSpacing = realHeight / height;

        // Update offsets
        XOffset = width % 2 == 0 ? HorizontalSpacing / 2 : 0;
        YOffset = height % 2 == 0 ? VerticalSpacing / 2 : 0;
    }
    
    protected virtual TCell GetClosest(Vector2 position)
    {
        Vector2 distance = position - (Vector2)transform.position;
            
        // Position is out of bounds
        if (Mathf.Abs(distance.x) > HorizontalSpacing * width / 2 ||
            Mathf.Abs(distance.y) > VerticalSpacing * height / 2)
            return null;

        float horizontalBound = width / 2f;
        float verticalBound = height / 2f;

        float x = position.x / HorizontalSpacing;
        float y = position.y / VerticalSpacing;
            
        var gridX = (int)(x + horizontalBound);
        var gridY = (int)(y + verticalBound);

        // Validate the position
        bool inBound = gridX < width && gridX >= 0 && gridY < height && gridY >= 0;

        return inBound ? Cells[gridX, gridY] : null;
    }
    
    public TCell[,] Cells { get; protected set; }

    protected virtual void OnStart()
    {
        Cells = new TCell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cells[i, j] = default;
            }
        }
    }
    protected virtual void Validate()
    {
        if (!Application.isPlaying || Cells == null) return;

        width = Cells.GetLength(0);
        height = Cells.GetLength(1);
    }
    protected virtual void OnGizmos() {}
    
    private void Start() => OnStart();
    private void OnDrawGizmos() => OnGizmos();
    private void OnValidate() => Validate();
}

using _Project.Scripts.Grid;
using UnityEngine;

public class Grid<T> : MonoBehaviour where T : Cell
{
    public int Width => width;
    public int Height => height;
    
    [SerializeField] protected int width;
    [SerializeField] protected int height;

    /// <summary>
    /// The width of one grid cell (in world units)
    /// </summary>
    public float HorizontalSpacing { get; protected set; }
    /// <summary>
    /// The height of one grid cell (in world units)
    /// </summary>
    public float VerticalSpacing { get; protected set; }
        
    /// <summary>
    /// The horizontal offset of each cell (in world units)
    /// </summary>
    public float XOffset { get; protected set; }
    /// <summary>
    /// The vertical offset of each cell (in world units)
    /// </summary>
    public float YOffset { get; protected set; }
    
    public virtual Vector2 FromGridPosition(Vector2Int gridPosition) { return Vector2.zero; }
    
    public T[,] Cells { get; protected set; }

    protected virtual void OnStart()
    {
        Cells = new T[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cells[i, j] = default;
                Cells[i, j]?.Initialize();
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

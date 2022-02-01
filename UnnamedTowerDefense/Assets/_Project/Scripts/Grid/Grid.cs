using System;
using _Project.Scripts.Grid;
using UnityEngine;

public class Grid<T> : MonoBehaviour where T : Cell
{
    [SerializeField] protected int width;
    [SerializeField] protected int height;

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

using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Path_System;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    private bool initialized;
    public PathFile reference;

    private List<Vector2> _points;
    private List<Vector2> Points
    {
        get => _points;
        set
        {
            _points = value;

            // Get the points as a Vector3 array
            List<Vector3> pointsV3 = new();
            value.ForEach(p => pointsV3.Add(p));

            if (!initialized)
            {
                Initialize(value);
                return;
            }
            
            line.positionCount = pointsV3.Count;
            line.SetPositions(pointsV3.ToArray());
        }
    }

    private LineRenderer line;

    /// <summary>
    /// Initialize the path
    /// </summary>
    /// <param name="positions">The list of positions to start the path with</param>
    private void Initialize(IEnumerable<Vector2> positions)
    {
        line = GetComponent<LineRenderer>();
        initialized = true;

        transform.position = Vector2.zero;
        Points = positions.ToList();
    }

    /// <summary>
    /// Update the points to fit <see cref="reference"/>.
    /// </summary>
    public void UpdatePoints() =>
            Points = PathInterpreter.PositionsFromFile(reference).ToList();

    [MenuItem("GameObject/2D Object/Path", priority = 10)]
    public static Path New() => New("Unnamed Path");
    public static Path New(string name) => New(Array.Empty<Vector2>(), name);
    public static Path New(IEnumerable<Vector2> startPositions, string name = "Unnamed Path")
    {
        // Stays unnamed and in default directory
        var pathFile = PathFile.Create();
        
        var result = new GameObject(name,typeof(LineRenderer), typeof(Path)).GetComponent<Path>();
        result.reference = pathFile;
        
        result.Initialize(startPositions);

        return result;
    }
    
    /// <summary>
    /// Add a point (position) to the path
    /// </summary>
    /// <param name="x">The x position of the new point</param>
    /// <param name="y">The y position of the new point</param>
    public void LineTo(float x, float y) => LineTo(new Vector2(x, y));
    
    /// <summary>
    /// Add a point (position) to the path
    /// </summary>
    /// <param name="position">The <see cref="Vector2"/> position of the new point.</param>
    public void LineTo(Vector2 position)
    {
        var newPoints = Points;
        newPoints.Add(position);

        Points = newPoints;
    }
}

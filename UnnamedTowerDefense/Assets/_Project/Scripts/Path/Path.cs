using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<PathPoint> points = new List<PathPoint>();

    [Readonly]
    public List<PathLine> lines = new List<PathLine>();

    private void Start() => MakeLines();

    public PathPoint NewPoint(Vector2 position)
    {
        PathPoint point = PathPoint.New(position);
        point.name = "Path Point";

        point.transform.position = position;
        point.transform.SetParent(transform);

        points.Add(point);

        return point;
    }

    public void MakeLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i]);
            lines.RemoveAt(i);
        }
        lines.Clear();

        for (int i = 0; i < points.Count - 1; i++)
        {
            PathLine line = PathLine.New(points[i], points[i + 1]);
            lines.Add(line);
        }
    }
}
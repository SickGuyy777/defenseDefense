using UnityEngine;
using System.Collections;

public class PathLine : MonoBehaviour
{
    public static GameObject PathLinePrefab =>
            Prefabs.prefabs["Path-Line"];

    public LineRenderer Line { get; private set; }

    public PathPoint StartPoint { get; private set; }
    public PathPoint EndPoint { get; private set; }

    public void Initialize(PathPoint startPoint, PathPoint endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;


        Line = gameObject.GetComponent<LineRenderer>();
        if (!Line)
            Line = gameObject.AddComponent<LineRenderer>();

        Line.SetPositions(new Vector3[2] { StartPoint, EndPoint });
    }

    public static PathLine New(PathPoint startPoint, PathPoint endPoint)
    {
        Vector2 start = startPoint.Position;
        Vector2 end = endPoint.Position;

        Vector2 position = new Vector2((start.x + end.x) / 2,
                (start.y + end.y) / 2);

        PathLine result = Instantiate(PathLinePrefab, position,
                Quaternion.identity).GetComponent<PathLine>();
        result.Initialize(startPoint, endPoint);

        return result;
    }
}

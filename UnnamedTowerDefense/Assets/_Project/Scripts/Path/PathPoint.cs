using UnityEngine;

[DefaultExecutionOrder(-int.MaxValue)]
public class PathPoint : MonoBehaviour
{
    public static GameObject PointPrefab => Prefabs.prefabs["Path-Point"];

    public Vector2 Position => transform.position;

    /// <summary>
    /// My <see cref="SpriteRenderer"/>.
    /// </summary>
    public SpriteRenderer Rend { get; private set; }

    private void Awake() => Rend = GetComponent<SpriteRenderer>();

    public static PathPoint New(Vector2 position) =>
        Instantiate(PointPrefab, position,
            Quaternion.identity).GetComponent<PathPoint>();

    public static implicit operator Vector2(PathPoint pp) => pp.Position;
    public static implicit operator Vector3(PathPoint pp) => pp.Position;

    public static explicit operator PathPoint(Vector3 position) =>
        New(position);
    public static explicit operator PathPoint(Vector2 position) =>
        New(position);
}
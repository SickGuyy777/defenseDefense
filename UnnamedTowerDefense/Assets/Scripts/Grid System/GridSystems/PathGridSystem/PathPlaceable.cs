using UnityEngine;

public class PathPlaceable : MonoBehaviour
{
    [SerializeField] private string placeableName;
    public string PlaceableName => placeableName;
    
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    public static implicit operator Sprite(PathPlaceable pp) => pp.sprite;
}

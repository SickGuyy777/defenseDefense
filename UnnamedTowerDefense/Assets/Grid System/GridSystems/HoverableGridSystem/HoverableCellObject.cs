using UnityEditor;
using UnityEngine;

namespace Grid_System
{
    public class HoverableCellObject :
            CellObject<HoverableCell, HoverableGrid, HoverableCellProperties, HoverableCellObject>
    {
        public SpriteRenderer Renderer { get; protected set; }

        private static Sprite _defaultSprite;
        private static Sprite DefaultSprite
        {
            get
            {
                const string path = @"Assets/Grid System/GridSystems/HoverableGridSystem/Sprites/CellPlaceholder.png";
                if (_defaultSprite == null)
                    _defaultSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                return _defaultSprite;
            }
        }

        private static Sprite _rotatedSprite;
        private static Sprite RotatedSprite
        {
            get
            {
                const string path =
                    @"Assets/Grid System/GridSystems/HoverableGridSystem/Sprites/CellPlaceholderRotated.png";

                if (_rotatedSprite == null)
                    _rotatedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                return _rotatedSprite;
            }
        }

        private void Awake()
        {
            Renderer = gameObject.AddComponent<SpriteRenderer>();
            Renderer.sprite = DefaultSprite;
            
            // Reset sprite color
            Color c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, c.b, 0f);

            transform.localScale = Cell.Parent.CellScale;
        }

        protected override void OnInitialize()
        {
            transform.position = Cell.WorldPosition;
            transform.SetParent(Cell.Parent.transform);
            
            Cell.OnHover += OnHover;
            Cell.OnStopHover += OnStopHover;
            Cell.OnRotate += OnRotate;
        }

        private void OnRotate(int _, int newState)
        {
            // Vector3 rot = transform.eulerAngles;
            // transform.localRotation = Quaternion.Euler(rot.x, rot.y, newState * 90);
            
            // Switch sprite
            Sprite newSprite = newState == 0 ? DefaultSprite : RotatedSprite;
            Renderer.sprite = newSprite;
        }

        private void OnStopHover()
        {
            Color c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, c.b, 0f);
        }

        private void OnHover()
        {
            Color c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, c.b, 0.5f);
        }
    }
}
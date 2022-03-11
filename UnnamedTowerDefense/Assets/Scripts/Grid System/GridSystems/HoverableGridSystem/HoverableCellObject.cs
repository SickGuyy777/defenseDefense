using UnityEngine;

namespace Grid_System
{
    public class HoverableCellObject :
            CellObject<HoverableCell, HoverableGrid, HoverableCellProperties, HoverableCellObject>
    {
        public SpriteRenderer Renderer { get; protected set; }
        public Sprite Sprite
        {
            get => Renderer.sprite;
            set => Renderer.sprite = value;
        }

        public Color Color
        {
            get => Renderer.color;
            set => Renderer.color = value;
        }

        public void SetAlpha(float alpha)
        {
            alpha = Mathf.Clamp01(alpha);

            Color c = Color;
            Color = new Color(c.r, c.g, c.b, alpha);
        }
        
        private void Awake()
        {
            Renderer = gameObject.AddComponent<SpriteRenderer>();

            // Reset sprite color
            Color c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, c.b, 0f);
        }

        protected override void OnInitialize()
        {
            transform.position = Cell.WorldPosition;
            transform.SetParent(Cell.Parent.transform);
            
            transform.localScale = Cell.Parent.CellScale;
            Cell.Properties = new HoverableCellProperties();
        }
    }
}
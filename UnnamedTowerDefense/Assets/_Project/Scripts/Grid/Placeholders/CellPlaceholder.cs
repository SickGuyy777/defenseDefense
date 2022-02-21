using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Grid.Placeholders
{
    public class CellPlaceholder<TCell, TGrid, TThis> : MonoBehaviour
        where TCell : Cell<TGrid, TThis, TCell>
        where TGrid : Grid<TCell, TThis, TGrid>
        where TThis : CellPlaceholder<TCell, TGrid, TThis>
    {
        public const string DefaultSpritePath = "Assets/_Project/Sprites/Grid/Cell/Cell Placeholder.png";
        
        public  SpriteRenderer Renderer { get; private set; }


        private static Sprite _defaultSprite;
        public static Sprite DefaultSprite
        {
            get
            {
                if (!_defaultSprite)
                    _defaultSprite = GetDefaultSprite();

                return _defaultSprite;
            }
        }
        
        public TCell Cell { get; protected set; }

        protected virtual void OnAwake() {}

        private void Awake()
        {
            // Get SpriteRenderer
            Renderer = GetComponent<SpriteRenderer>();

            if (!Renderer)
                Renderer = gameObject.AddComponent<SpriteRenderer>();

            // Set the default sprite
            Renderer.sprite = DefaultSprite;
            
            OnAwake();
        }

        #region Renderer Color Controller
        /// <summary>
        /// Set my <see cref="Renderer"/>'s alpha value.
        /// </summary>
        /// <param name="alpha">The new alpha value. Ranges: 0 - 1</param>
        public void SetAlpha(float alpha)
        {
            CheckColorValue(alpha);

            Color color = Renderer.color;
            Renderer.color = new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Set my <see cref="Renderer"/>'s red color value.
        /// </summary>
        /// <param name="red">The new red color value. Range: 0 - 1</param>
        public void SetRed(float red)
        {
            CheckColorValue(red);

            Color c = Renderer.color;
            Renderer.color = new Color(red, c.g, c.b, c.a);
        }
        
        /// <summary>
        /// Set my <see cref="Renderer"/>'s green color value.
        /// </summary>
        /// <param name="green">The new green color value. Range: 0 - 1</param>
        public void SetGreen(float green)
        {
            CheckColorValue(green);

            Color c = Renderer.color;
            Renderer.color = new Color(c.r, green, c.b, c.a);
        }
        
        /// <summary>
        /// Set my <see cref="Renderer"/>'s blue color value.
        /// </summary>
        /// <param name="blue">The new blue color value. Range: 0 - 1</param>
        public void SetBlue(float blue)
        {
            CheckColorValue(blue);

            Color c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, blue, c.a);
        }

        private static void CheckColorValue(float value)
        {
            if (value is > 1 or < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Green value is out of bounds. (0-1)");
        }
        #endregion
        #region Default Sprite
        /// <summary>
        /// Get the default sprite of a cell placeholder.
        /// </summary>
        /// <returns>The default sprite of a cell placeholder</returns>
        private static Sprite GetDefaultSprite() => AssetDatabase.LoadAssetAtPath<Sprite>(DefaultSpritePath);
        #endregion

        public void Initialize(TCell cell) { }
        public static TThis New(TCell cell) => default;
    }
}
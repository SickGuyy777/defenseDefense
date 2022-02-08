using _Project.Scripts.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Grid
{
    public class WorldCellPlaceholder : MonoBehaviour
    {
        private static Sprite _defaultSprite;
        private static Sprite DefaultSprite
        {
            get
            {
                if (!_defaultSprite)
                    _defaultSprite = GetDefaultSprite();

                return _defaultSprite;
            }
        }
        
        public WorldCell Cell { get; private set; }
        public SpriteRenderer Renderer { get; private set; }
        public BoxCollider2D Collider { get; private set; }

        [Readonly]
        [SerializeField]
        private GameGrid grid;

        private void Awake()
        {
            // Assert if the cell is null
            if (Cell == null)
            {
                Debug.LogError("Cell is null!");
                return;
            }

            Renderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<BoxCollider2D>();
            
            // If there is no SpriteRenderer on the GameObject, add it.
            if (!Renderer)
                Renderer = gameObject.AddComponent<SpriteRenderer>();
            
            // If there is no BoxCollider2D on the GameObject, add it.
            if (!Collider)
                Collider = gameObject.AddComponent<BoxCollider2D>();
            
            // Set the default sprite
            Renderer.sprite = DefaultSprite;

            // Set the collider's size
            Collider.size = new Vector2(transform.localScale.x / grid.HorizontalSpacing, transform.localScale.y / grid.VerticalSpacing);
        }

        /// <summary>
        /// Initialize the placeholder with <paramref name="cell"/>.
        /// </summary>
        /// <param name="cell">The reference <see cref="WorldCell"/>.</param>
        public void Initialize(WorldCell cell)
        {
            // Assert if the cell is null
            if (cell == null)
            {
                Debug.LogError("Cannot initialize placeholder if the cell is null.", this);
                return;
            }

            // Set placeholder properties
            Cell = cell;
            grid = (GameGrid)cell.Parent;

            // Set GamObject properties
            name = cell.GridPosition.ToString();
            transform.position = cell.WorldPosition;
            transform.localScale = new Vector2(grid.HorizontalSpacing, grid.VerticalSpacing);

            // Set as child of the grid
            transform.SetParent(cell.Parent.transform);
        }

        /// <summary>
        /// Create a new <see cref="WorldCellPlaceholder"/>.
        /// </summary>
        /// <param name="cell">The <see cref="WorldCell"/> reference to create a <see cref="WorldCellPlaceholder"/>.</param>
        /// <returns>The created <see cref="WorldCellPlaceholder"/></returns>
        public static WorldCellPlaceholder New(WorldCell cell)
        {
            // Create a new object
            var go = new GameObject(string.Empty);
            
            // Deactivate the object so awake doesn't trigger
            go.SetActive(false);
            
            // Add the component
            var placeholder = go.AddComponent<WorldCellPlaceholder>();
            
            // Initialize before awake
            placeholder.Initialize(cell);
            
            // Re-activate the GameObject to trigger awake
            go.SetActive(true);

            // Initialize the placeholder
            placeholder.Initialize(cell);

            return placeholder;
        }

        /// <summary>
        /// Get the default sprite of a cell placeholder.
        /// </summary>
        /// <returns>The default sprite of a cell placeholder</returns>
        private static Sprite GetDefaultSprite()
        {
            const string path = "Assets/_Project/Sprites/Grid/Cell/Cell Placeholder.png";
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        #region Renderer Color Controller
        /// <summary>
        /// Set my <see cref="Renderer"/>'s alpha value.
        /// </summary>
        /// <param name="alpha">The new alpha value. Ranges: 0 - 1</param>
        public void SetAlpha(float alpha)
        {
            if (alpha is > 1 or < 0)
            {
                Debug.LogError("Invalid alpha value.");
                return;
            }

            var color = Renderer.color;
            Renderer.color = new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Set my <see cref="Renderer"/>'s red color value.
        /// </summary>
        /// <param name="red">The new red color value. Range: 0 - 1</param>
        public void SetRed(float red)
        {
            Assert.IsFalse(red is > 1 or < 0, "Invalid red color value.");

            var c = Renderer.color;
            Renderer.color = new Color(red, c.g, c.b, c.a);
        }
        
        /// <summary>
        /// Set my <see cref="Renderer"/>'s green color value.
        /// </summary>
        /// <param name="green">The new green color value. Range: 0 - 1</param>
        public void SetGreen(float green)
        {
            Assert.IsFalse(green is > 1 or < 0, "Invalid green color value.");

            var c = Renderer.color;
            Renderer.color = new Color(c.r, green, c.b, c.a);
        }
        
        /// <summary>
        /// Set my <see cref="Renderer"/>'s blue color value.
        /// </summary>
        /// <param name="blue">The new blue color value. Range: 0 - 1</param>
        public void SetBlue(float blue)
        {
            Assert.IsFalse(blue is > 1 or < 0, "Invalid blue color value.");

            var c = Renderer.color;
            Renderer.color = new Color(c.r, c.g, blue, c.a);
        }
        #endregion
    }
};
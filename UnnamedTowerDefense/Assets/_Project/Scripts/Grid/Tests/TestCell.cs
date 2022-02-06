using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;
using CellUpdater = _Project.Scripts.Grid.Tests.TestCellUpdater;

namespace _Project.Scripts.Grid.Tests
{
    public class TestCell : Cell
    {
        private const string CellSpritePath = "Assets/_Project/Sprites/Grid/Cell/Temp/Cell Placeholder.png";
        
        public delegate void TestCellEvent(TestCell objectCell);
        public event TestCellEvent OnClicked;
        
        public Grid<TestCell> Parent { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        

        private GameObject _occupier;
        public GameObject Occupier
        {
            get => _occupier;
            private set
            {
                if (value == _occupier)
                    return;
                
                _occupier = value;
                UpdatePlaceholder();
            }
        }

        
        private CellUpdater placeHolder;
        private SpriteRenderer renderer;

        public bool IsOccupied => Occupier;

        public void Leave()
        {
            if (!IsOccupied)
            {
                Debug.LogError("Cell is not occupied!");
                return;
            }

            Object.Destroy(Occupier.GetComponent<PositionConstraint>());
            Occupier = null;
        }
        
        public bool Occupy(GameObject occupier)
        {
            if (IsOccupied)
            {
                Debug.LogError("The cell is currently occupied.");
                return false;
            }
            
            Occupier = occupier;
            
            occupier.transform.position = WorldPosition;
            occupier.AddComponent<PositionConstraint>().constraintActive = true;
            
            return true;
        }

        public float HorizontalSpace { get; private set; }
        public float VerticalSpace { get; private set; }

        public void Initialize(int x, int y, float horizontalSpace, float verticalSpace, Grid<TestCell> parent)
            => Initialize(new Vector2Int(x, y), horizontalSpace, verticalSpace, parent);

        public void Initialize(Vector2Int position, float horizontalSpace, float verticalSpace, Grid<TestCell> parent)
        {
            HorizontalSpace = horizontalSpace;
            VerticalSpace = verticalSpace;
            Parent = parent;

            GridPosition = position;
            WorldPosition = parent.FromGridPosition(GridPosition);

            placeHolder = new GameObject($"Click Tester ({GridPosition})", typeof(CellUpdater))
            {
                transform =
                {
                    position = WorldPosition,
                    localScale = new Vector3(horizontalSpace, verticalSpace, 0)
                }
            }.GetComponent<CellUpdater>();
            
            placeHolder.transform.SetParent(parent.transform);
            renderer = placeHolder.spriteRenderer;

            var sprite = GetCellSprite();
            renderer.sprite = sprite;
            
            placeHolder.SetAlpha(0);
            
            placeHolder.OnLeave += () => placeHolder.SetAlpha(0);
            placeHolder.OnHover += UpdatePlaceholder;
            placeHolder.OnClick += () => OnClicked?.Invoke(this);
        }

        public TestCell(int x, int y, float horizontalSpace, float verticalSpace, Grid<TestCell> parent) =>
                Initialize(x, y, horizontalSpace, verticalSpace, parent);
        public TestCell(Vector2Int position, float horizontalSpace, float verticalSpace, Grid<TestCell> parent) =>
                Initialize(position, horizontalSpace, verticalSpace, parent);

        private void UpdatePlaceholder() => SetRed(IsOccupied ? 255 : 0);

        private void SetRed(float r, float a = 0.25f) => placeHolder.SetColor(new Color(r, 0, 0, a));
        private void SetAlpha(float a) => placeHolder.SetAlpha(a);
        
        private static Sprite GetCellSprite() =>
                AssetDatabase.LoadAssetAtPath<Sprite>(CellSpritePath);
    }
}
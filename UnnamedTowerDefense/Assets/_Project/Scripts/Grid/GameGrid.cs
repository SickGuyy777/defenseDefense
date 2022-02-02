using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Grid
{
    public class GameGrid : Grid<ObjectCell>
    {
        [SerializeField] private Sprite square;

        // ReSharper disable once InconsistentNaming 
        private float xOffset;
        // ReSharper disable once InconsistentNaming
        private float yOffset;

        // ReSharper disable once InconsistentNaming
        private float horizontalSpace;
        // ReSharper disable once InconsistentNaming
        private float verticalSpace;
        
        public override Vector2 FromGridPosition(Vector2Int gridPosition)
        {
            var x = (gridPosition.x - width / 2) * horizontalSpace + xOffset;
            var y = (gridPosition.y - height / 2) * verticalSpace + yOffset;

            return new Vector2(x, y);
        }

        protected override void OnStart() => Initialize();
        
        private void Initialize()
        {
            Cells = new ObjectCell[width, height];

            Camera cam = Camera.main;
            Assert.IsNotNull(cam, "Main camera is not available.");
            
            var screenHeight = cam!.orthographicSize * 2f;
            var screenWidth = cam.aspect * screenHeight;

            horizontalSpace = screenWidth / width;
            verticalSpace = screenHeight / height;

            xOffset = width % 2 == 0 ? horizontalSpace / 2 : 0;
            yOffset = height % 2 == 0 ? verticalSpace / 2 : 0;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cells[i, j] = new ObjectCell(new Vector2Int(i, j), horizontalSpace, verticalSpace, this);
                    Cells[i, j].OnClicked += OnClick;
                }
            }
        }

        private void OnClick(ObjectCell clicked)
        {
            if (clicked.IsOccupied)
            {
                Destroy(clicked.Occupier);
                clicked.Leave();
                
                return;
            }

            var obj = new GameObject("Occupier", typeof(SpriteRenderer))
            {
                transform = { localScale = new Vector3(horizontalSpace, verticalSpace) }
            };
            var rend = obj.GetComponent<SpriteRenderer>();
            rend.sprite = square;
            rend.sortingOrder = -1;

            clicked.Occupy(obj);
        }
    }
}
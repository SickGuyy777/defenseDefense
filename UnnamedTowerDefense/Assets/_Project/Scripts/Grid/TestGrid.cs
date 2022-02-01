using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Grid
{
    public class TestGrid : Grid<TestCell>
    {
        protected override void Validate()
        {
            if (!Application.isPlaying || Cells == null || width < 0 || height < 0)
                return;

            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j].Destroy();
                }
            }
            
            Initialize();
        }

        protected override void OnStart() => Initialize();

        private void Initialize()
        {
            Cells = new TestCell[width, height];

            Camera cam = Camera.main;
            Assert.IsNotNull(cam, "Main camera is not available.");
            
            float screenHeight = cam!.orthographicSize * 2f;
            float screenWidth = cam.aspect * screenHeight;

            float horizontalSpace = screenWidth / width;
            float verticalSpace = screenHeight / height;

            float xOffset = width % 2 == 0 ? horizontalSpace / 2 : 0;
            float yOffset = height % 2 == 0 ? verticalSpace / 2 : 0;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var x = (i - width / 2) * horizontalSpace + xOffset;
                    var y = (j - height / 2) * verticalSpace + yOffset;

                    Cells[i, j] = new TestCell(x, y, horizontalSpace, verticalSpace);
                }
            }
        }

        protected override void OnGizmos()
        {
            if (Cells == null || Cells.GetLength(0) != width || Cells.GetLength(1) != height)
                return;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Cells[i, j] != null)
                        Cells[i, j]?.DrawGizmos();
                }
            }
        }
    }
}
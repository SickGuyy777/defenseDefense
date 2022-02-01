using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Grid
{
    public class TestCell : Cell
    {
        private Vector2 position;
        private GameObject go;
        public float HorizontalSpace { get; private set; }
        public float VerticalSpace { get; private set; }

        private new void Initialize(params object[] parameters) => throw new NotImplementedException();

        public void Initialize(float x, float y, float horizontalSpace, float verticalSpace)
            => Initialize(new Vector2(x, y), horizontalSpace, verticalSpace);

        public void Initialize(Vector2 position, float horizontalSpace, float verticalSpace)
        {
            Destroy();
            
            HorizontalSpace = horizontalSpace;
            VerticalSpace = verticalSpace;
            this.position = position;

            go = new GameObject(position.ToString())
            {
                transform = { position = this.position }
            };
        }

        public TestCell(float x, float y, float horizontalSpace, float verticalSpace) =>
                Initialize(x, y, horizontalSpace, verticalSpace);
        public TestCell(Vector2 position, float horizontalSpace, float verticalSpace) =>
                Initialize(position, horizontalSpace, verticalSpace);

        public void Destroy()
        {
            if (go)
                Object.Destroy(go);
        }

        public void DrawGizmos()
        {
            // Left
            Vector2 bottomLeft = new Vector2(position.x - HorizontalSpace / 2, position.y  - VerticalSpace / 2);
            Vector2 topLeft = new Vector2(position.x - HorizontalSpace / 2, position.y + VerticalSpace / 2);
            Gizmos.DrawLine(bottomLeft, topLeft);
            
            // Right
            Vector2 bottomRight = new Vector2(position.x + HorizontalSpace / 2, position.y - VerticalSpace / 2);
            Vector2 topRight = new Vector2(position.x + HorizontalSpace / 2, position.y + VerticalSpace / 2);
            Gizmos.DrawLine(bottomRight, topRight);

            // Top
            Gizmos.DrawLine(topLeft, topRight);
            
            // Bottom
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }
}
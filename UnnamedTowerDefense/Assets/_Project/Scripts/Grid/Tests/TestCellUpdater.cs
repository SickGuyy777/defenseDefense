// using System;
// using UnityEngine;
//
// namespace _Project.Scripts.Grid.Tests
// {
//     [AddComponentMenu("Tests/Grid/Cell Updater")]
//     public class TestCellUpdater : MonoBehaviour
//     {
//         public event Action OnClick;
//         public event Action OnHover;
//         public event Action OnLeave;
//
//         [HideInInspector] public SpriteRenderer spriteRenderer;
//         
//         private void OnEnable()
//         {
//             if (!GetComponent<BoxCollider2D>())
//                 gameObject.AddComponent<BoxCollider2D>();
//
//             spriteRenderer = GetComponent<SpriteRenderer>();
//             if (!spriteRenderer)
//                 spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
//         }
//
//         public void SetColor(Color c) => spriteRenderer.color = c;
//         public void SetAlpha(float a)
//         {
//             var c = spriteRenderer.color;
//             c.a = a;
//             
//             SetColor(c);
//         }
//
//         private void OnMouseExit() => OnLeave?.Invoke();
//         private void OnMouseEnter() => OnHover?.Invoke();
//         private void OnMouseDown() => OnClick?.Invoke();
//     }
// }
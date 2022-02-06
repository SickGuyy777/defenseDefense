using UnityEngine;

namespace _Project.Scripts.Cursor
{
    public static class Mouse
    {
        private static Camera _cam;
        public static Camera Cam
        {
            get
            {
                if (!_cam)
                    _cam = Camera.main;

                return _cam;
            }
        }

        public static Vector2 Position => Cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
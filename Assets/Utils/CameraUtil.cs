using UnityEngine;

namespace Fangtang.Utils
{
	public static class CameraUtil
    {
        public static readonly float DefaultOrthographicSize = 11.0f;

		public static Vector3 SwitchCameraWorldPostion(Camera srcCamera, Camera destCamera, Vector3 srcPosition)
		{
			Vector3 screenPosition = srcCamera.WorldToScreenPoint(srcPosition);
			return destCamera.ScreenToWorldPoint(screenPosition);
		}
		
		public static float GetCameraHalfWidth(Camera aCamera)
		{
			return aCamera.aspect * aCamera.orthographicSize;
		}

		public static float GetCameraHalfHeight(Camera aCamera)
		{
			return aCamera.orthographicSize;
		}

        public static Vector2 TransformSizeFromScreenToWorld(Camera anOrthoCamera, Vector2 screenSize)
        {
            float cameraSizeY = anOrthoCamera.orthographicSize;
            float cameraSizeX = cameraSizeY / Screen.height * Screen.width;

            Vector3 worldSize = Vector3.zero;
            worldSize.x = screenSize.x / Screen.width * cameraSizeX * 2;
            worldSize.y = screenSize.y / Screen.height * cameraSizeY * 2;
            return worldSize;
        }
	}
}


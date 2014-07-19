using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

namespace GameJam.Utils
{
    public static class GameObjectExtension
    {
        public static T GetComponentAndCreateIfNonExist<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static void SetLocalPositionX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        public static bool IsBumping(this CollisionFlags collisionFlags)
        {
            return (collisionFlags & (CollisionFlags.CollidedSides)) != 0;
        }

        public static bool IsGrounded(this CollisionFlags collisionFlags)
        {
            return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
        }

        public static bool IsHittingHead(this CollisionFlags collisionFlags)
        {
            return (collisionFlags & CollisionFlags.CollidedAbove) != 0;
        }

        public static void DestroyBasedOnRunning(this UnityEngine.Object anObject)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(anObject);
            }
            else
            {
                GameObject.DestroyImmediate(anObject);
            }
        }
    }
}

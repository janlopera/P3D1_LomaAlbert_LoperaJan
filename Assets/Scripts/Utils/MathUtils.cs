using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static Vector3 ToHorizontal(this Vector3 v)
        {
            return Vector3.ProjectOnPlane(v, Vector3.down);
        }

        public static float YComponent(this Vector3 v)
        {
            return Vector3.Dot(v, Vector3.up);
        }
        public static float XComponent(this Vector3 v)
        {
            return Vector3.Dot(v, Vector3.right);
        }
        
        public static float ZComponent(this Vector3 v)
        {
            return Vector3.Dot(v, Vector3.forward);
        }
	
        public static Vector3 TransformDirToHorizontal(this Transform t, Vector3 v)
        {
            return t.TransformDirection(v).ToHorizontal().normalized;
        }

        public static Vector3 InverseTransformDirToHorizontal(this Transform t, Vector3 v)
        {
            return t.InverseTransformDirection(v).ToHorizontal().normalized;
        }
        
        public static bool Between(this float num, float lower, float upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

    }
}

using UnityEngine;

public static class VectorExtensionMethods
{
    public static Vector3 Multiply(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector3 GetReciprocal(this Vector3 v)
    {
        return new Vector3(
            v.x != 0 ? 1f / v.x : 0f,
            v.y != 0 ? 1f / v.y : 0f,
            v.z != 0 ? 1f / v.z : 0f
        );
    }

    /// <summary>
    /// When you call Quaternion.eulerAngles, you get a value between 0 and 360 for each axis
    /// This changes the range to -180 to 180, which is often more useful for determining shortest rotation direction
    /// </summary>
    public static Vector3 DirectionalizeEulerAngles(this Vector3 eulerAngles)
    {
        return new Vector3(
            eulerAngles.x > 180f ? eulerAngles.x - 360f : eulerAngles.x,
            eulerAngles.y > 180f ? eulerAngles.y - 360f : eulerAngles.y,
            eulerAngles.z > 180f ? eulerAngles.z - 360f : eulerAngles.z
        );
    }
}

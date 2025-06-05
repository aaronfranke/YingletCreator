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
}

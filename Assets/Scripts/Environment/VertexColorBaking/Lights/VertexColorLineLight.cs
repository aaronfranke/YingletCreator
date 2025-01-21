using System;
using UnityEngine;

public class VertexColorLineLight : MonoBehaviour, IVertexColorLight
{
	[ColorUsage(showAlpha: true, hdr: true)]
	[SerializeField] Color Color = Color.white;
	[SerializeField] float Intensity = 1;
	[SerializeField] float Range = 3;
	[SerializeField] float Width = 3;

	const float GizmoThickness = .3f;

	Color LowAlphaColor
	{
		get
		{
			var color = Color;
			color.a = .3f;
			return color;
		}
	}

	Color IVertexColorLight.Color => Color;

	float IVertexColorLight.Intensity => Intensity;

	float IVertexColorLight.Range => Range;

	public float GetDistance(Vector3 from)
	{
		Vector3 right = this.transform.right * Width / 2;
		Vector3 a = this.transform.position - right;
		Vector3 b = this.transform.position + right;
		return DistancePointToLineSegment(from, a, b);
	}

	public static float DistancePointToLineSegment(Vector3 point, Vector3 a, Vector3 b) // ChatGPT generated
	{
		Vector3 ab = b - a;
		Vector3 ap = point - a;

		float magnitudeAB = ab.sqrMagnitude; // Squared length of segment
		if (magnitudeAB == 0.0f)
			return Vector3.Distance(point, a); // a and b are the same point

		float t = Vector3.Dot(ap, ab) / magnitudeAB; // Projection factor

		// Clamp t to stay within the segment
		t = Mathf.Clamp01(t);

		// Compute the closest point on the segment
		Vector3 closestPoint = a + t * ab;

		// Return the distance from the point to the closest point on the segment
		return Vector3.Distance(point, closestPoint);
	}

	void OnDrawGizmos()
	{
		DrawGizmos(false);
	}

	void OnDrawGizmosSelected()
	{
		DrawGizmos(true);
	}

	void DrawGizmos(bool selected)
	{
		// Store original matrix
		Matrix4x4 originalMatrix = Gizmos.matrix;

		// Apply transformation
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

		// Draw cube
		Gizmos.color = selected ? LowAlphaColor : Color;

		if (selected)
		{
			Gizmos.DrawWireCube(Vector3.zero, Vector3.right * Width + Vector3.one * Range);
		}
		else
		{
			Gizmos.DrawCube(Vector3.zero, Vector3.right * Width + Vector3.one * GizmoThickness);
		}

		// Restore original matrix
		Gizmos.matrix = originalMatrix;
	}
}



using UnityEngine;

public interface IVertexColorLight
{
	public Color Color { get; }
	public float Intensity { get; }
	public float Range { get; }
	public float GetDistance(Vector3 from);
}
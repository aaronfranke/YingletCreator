using Character.Compositor;
using UnityEngine;

/// <summary>
/// Provides various values relating to height.
/// Useful for positioning cameras pointing at eye level
/// </summary>
public interface IYingletHeightProvider
{
	float YScale { get; }
}

public class YingletHeightProvider : MonoBehaviour, IYingletHeightProvider
{
	public float YScale => _compositedYingletRoot.lossyScale.y;
	private Transform _compositedYingletRoot;

	void Awake()
	{
		_compositedYingletRoot = this.GetComponentInChildren<CompositedYingletRoot>().transform;
	}
}

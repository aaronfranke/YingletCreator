using Reactivity;
using System.Linq;
using UnityEngine;

public interface IColliderHoverManager
{
	IHoverable CurrentlyHovered { get; }
}

public class ColliderHoverManager : MonoBehaviour, IColliderHoverManager
{
	const float MaxRaycastDistance = 50f;

	Observable<IHoverable> _currentlyHovered = new();

	public IHoverable CurrentlyHovered => _currentlyHovered.Val;

	private void Update()
	{
		// Ray from camera through mouse position
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		var maxDistance = GetMaxDistance(ray);

		var itemsHit = Physics.RaycastAll(ray, maxDistance);
		var firstHoverableHit = itemsHit
			.Select(itemsHit => itemsHit.collider.GetComponent<IHoverable>())
			.FirstOrDefault(hit => hit != null);
		_currentlyHovered.Val = firstHoverableHit;
	}

	float GetMaxDistance(Ray ray)
	{
		// Find intersection with XZ plane (y=0)
		float tPlane = 0f;
		if (Mathf.Abs(ray.direction.y) > 1e-5f)
			tPlane = -ray.origin.y / ray.direction.y;
		else
			tPlane = float.PositiveInfinity;

		// Clamp max distance to 50 units or intersection with XZ plane
		float maxDistance = Mathf.Min(MaxRaycastDistance, tPlane > 0 ? tPlane : MaxRaycastDistance);

		return maxDistance;
	}
}

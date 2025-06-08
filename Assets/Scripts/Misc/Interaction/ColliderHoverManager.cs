using Reactivity;
using System.Linq;
using UnityEngine;

public interface IColliderHoverManager
{
	IColliderHoverable CurrentlyHovered { get; }
}

public class ColliderHoverManager : MonoBehaviour, IColliderHoverManager
{
	const float MaxRaycastDistance = 50f;

	Observable<IColliderHoverable> _currentlyHovered = new();
	private IUiHoverManager _uiHoverManager;

	public IColliderHoverable CurrentlyHovered => _currentlyHovered.Val;

	void Awake()
	{
		_uiHoverManager = Singletons.GetSingleton<IUiHoverManager>();
	}

	private void Update()
	{
		if (_uiHoverManager.HoveringUi)
		{
			// If we're hovering UI, we shouldn't be hovering colliders
			_currentlyHovered.Val = null;
			return;
		}

		// Ray from camera through mouse position
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		var maxDistance = GetMaxDistance(ray);

		var itemsHit = Physics.RaycastAll(ray, maxDistance);
		var firstHoverableHit = itemsHit
			.Select(itemsHit => itemsHit.collider.GetComponentInParent<IColliderHoverable>())
			.Where(i => i != null)
			.Distinct()
			.OrderByDescending(i => i.PriorityFudge)
			.FirstOrDefault();
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

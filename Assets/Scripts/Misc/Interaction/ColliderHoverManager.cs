using Reactivity;
using System;
using System.Linq;
using UnityEngine;

public interface IColliderHoverManager
{
    IColliderHoverable CurrentlyHovered { get; }

    IDisposable DisableHovering();
}

public class ColliderHoverManager : MonoBehaviour, IColliderHoverManager
{
    const float MaxRaycastDistance = 50f;

    Observable<IColliderHoverable> _currentlyHovered = new();
    private IUiHoverManager _uiHoverManager;
    bool _disabled = false;

    public IColliderHoverable CurrentlyHovered => _currentlyHovered.Val;

    void Awake()
    {
        _uiHoverManager = Singletons.GetSingleton<IUiHoverManager>();
    }

    private void Update()
    {
        _currentlyHovered.Val = CalculateColliderHoverable();
    }

    IColliderHoverable CalculateColliderHoverable()
    {
        if (_disabled)
        {
            return null;
        }

        if (_uiHoverManager.HoveringUi)
        {
            // If we're hovering UI, we shouldn't be hovering colliders
            return null;
        }
        // Ray from camera through mouse position
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var maxDistance = GetMaxDistance(ray);

        var itemsHit = Physics.RaycastAll(ray, maxDistance);
        var firstHoverableHit = itemsHit
            .OrderBy(h => h.distance) // The order returned is undefined. Sort by distance first
            .Select(itemsHit => itemsHit.collider.GetComponentInParent<IColliderHoverable>())
            .Where(i => i != null)
            .Distinct()
            .OrderByDescending(i => i.PriorityFudge) // Also sort by priority fudge. This mantains order if equal
            .FirstOrDefault();

        return firstHoverableHit;
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

    public IDisposable DisableHovering()
    {
        _disabled = true;
        return new BasicActionDisposable(() =>
        {
            _disabled = false;
        });
    }
}

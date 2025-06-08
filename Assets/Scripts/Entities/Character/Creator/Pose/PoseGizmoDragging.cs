using Reactivity;
using System.Collections;
using UnityEngine;

public interface IPoseGizmo
{
	bool Dragging { get; }
}

public class PoseGizmoDragging : MonoBehaviour, IPoseGizmo
{
	IColliderHoverManager _hoverManager;
	private IPoseData _poseData;
	private IColliderHoverable _colliderHoverable;
	private Coroutine _activeDrag;
	Observable<bool> _dragging = new();

	public bool Dragging => _dragging.Val;

	private void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_poseData = this.GetComponentInParent<IPoseData>();
		_colliderHoverable = this.GetComponent<IColliderHoverable>();
	}
	void Update()
	{
		if (!Input.GetMouseButtonDown(0)) return;

		if (_activeDrag != null) return;

		if (_hoverManager.CurrentlyHovered != _colliderHoverable) return;

		_activeDrag = StartCoroutine(Drag());
	}

	IEnumerator Drag()
	{
		using var hoverDisable = _hoverManager.DisableHovering();
		_dragging.Val = true;
		var target = _poseData.CurrentlyEditing.GameObject.transform;

		// Store the initial offset between the target and the mouse hit point on the XZ plane
		Plane xzPlane = new Plane(Vector3.up, Vector3.zero);

		Vector3 initialTargetPos = target.position;
		Vector3 initialMouseWorldPos = GetMouseWorldPositionOnXZPlane(xzPlane);

		Vector3 offset = initialTargetPos - initialMouseWorldPos;

		while (Input.GetMouseButton(0))
		{
			Vector3 mouseWorldPos = GetMouseWorldPositionOnXZPlane(xzPlane);
			Vector3 newTargetPos = mouseWorldPos + offset;

			// Keep the target on the XZ plane (y=0)
			newTargetPos.y = 0f;
			target.position = newTargetPos;

			yield return null;
		}

		_activeDrag = null;
		_dragging.Val = false;
	}

	// Helper method to get the world position on the XZ plane under the mouse
	private Vector3 GetMouseWorldPositionOnXZPlane(Plane xzPlane)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float enter;
		if (xzPlane.Raycast(ray, out enter))
		{
			return ray.GetPoint(enter);
		}
		// Fallback: return zero if ray doesn't hit the plane (shouldn't happen in normal use)
		return Vector3.zero;
	}
}

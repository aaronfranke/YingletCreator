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
	private IPoseGizmoDragLogic _dragLogic;
	private IDragSfx _dragSfx;
	private Coroutine _activeDrag;
	Observable<bool> _dragging = new();

	public bool Dragging => _dragging.Val;

	private void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_poseData = this.GetComponentInParent<IPoseData>();
		_colliderHoverable = this.GetComponent<IColliderHoverable>();
		_dragLogic = this.GetComponent<IPoseGizmoDragLogic>();
		_dragSfx = this.GetComponent<IDragSfx>(); // should probably have another layer of separation via event instead but w/e
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
		float initialTargetRot = target.rotation.eulerAngles.y;

		Vector3 initialMousePos = GetMouseWorldPositionOnXZPlane(xzPlane);
		Vector3 lastMousePos = initialMousePos;

		while (Input.GetMouseButton(0))
		{
			// Calculate new mouse pos
			Vector3 currentMousePos = GetMouseWorldPositionOnXZPlane(xzPlane);
			// Play SFX if it has changed
			float distance = Vector3.Distance(currentMousePos, lastMousePos);
			if (distance > .01f) _dragSfx.Change(distance);
			// Cache the old position
			lastMousePos = currentMousePos;

			_dragLogic.UpdateTransform(target, initialMousePos, currentMousePos, initialTargetPos, initialTargetRot);

			yield return null;
		}

		this.transform.localRotation = Quaternion.identity;

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

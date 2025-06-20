using Character.Creator.UI;
using System.Collections.Generic;
using UnityEngine;

public class PoseModeCameraMovement : MonoBehaviour
{
	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] float _moveSmoothTime = 0.08f;
	[SerializeField] float _rotateSpeed = 30f;
	private IClipboardSelection _clipboardSelection;
	private Vector3 _eulerRotation;
	private Vector3 _targetPosition;
	private Vector3 _moveVelocity;

	static Dictionary<KeyCode, Vector3> _moveBindings = new()
		{
			{ KeyCode.W, Vector3.forward },
			{ KeyCode.S, Vector3.back },
			{ KeyCode.A, Vector3.left },
			{ KeyCode.D, Vector3.right },
			{ KeyCode.Q, Vector3.down },
			{ KeyCode.E, Vector3.up }
		};

	private void Awake()
	{
		_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
		_targetPosition = transform.position;
		_clipboardSelection.Selection.OnChanged += ClipboardSelection_OnChanged;
	}
	private void OnDestroy()
	{
		_clipboardSelection.Selection.OnChanged -= ClipboardSelection_OnChanged;
	}

	private void ClipboardSelection_OnChanged(ClipboardSelectionType from, ClipboardSelectionType to)
	{
		if (from != ClipboardSelectionType.Pose) return;
		// Reset the position/rotation
		_eulerRotation = transform.localEulerAngles;
		_targetPosition = transform.position;
	}

	void Update()
	{
		if (_clipboardSelection.Selection.Val != ClipboardSelectionType.Pose)
		{
			return;
		}

		UpdatePosition();
		UpdateRotation();
	}

	void UpdatePosition()
	{
		Vector3 direction = Vector3.zero;

		foreach (var binding in _moveBindings)
		{
			if (Input.GetKey(binding.Key))
			{
				direction += binding.Value;
			}
		}

		if (direction != Vector3.zero)
		{
			// Move relative to the transform's orientation
			Vector3 worldMove = transform.TransformDirection(direction);
			_targetPosition += worldMove.normalized * _moveSpeed * LimitedDeltaTime;
		}

		// Smoothly move towards the target position
		transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _moveVelocity, _moveSmoothTime);
	}

	private void UpdateRotation()
	{
		// Only rotate when right mouse button is held
		if (!Input.GetMouseButton(1)) return;

		float mouseX = Input.GetAxisRaw("Mouse X");
		float mouseY = Input.GetAxisRaw("Mouse Y");
		var rotateAngles = new Vector3(-mouseY, mouseX, 0);
		if (rotateAngles.magnitude < 0.001f) return;

		float rotateAmount = (LimitedDeltaTime * _rotateSpeed);
		_eulerRotation += rotateAngles * rotateAmount;
		transform.localRotation = Quaternion.Euler(_eulerRotation);
	}

	float LimitedDeltaTime => Mathf.Min(Time.deltaTime, 0.3f);
}

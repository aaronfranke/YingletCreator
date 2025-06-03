using Character.Creator.UI;
using System.Collections.Generic;
using UnityEngine;

public class PoseModeCameraMovement : MonoBehaviour
{
	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] float _rotateSpeed = 30f;
	private IClipboardSelection _clipboardSelection;
	private Vector3 _eulerRotation;
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
	}

	void Update()
	{
		if (_clipboardSelection.Selection.Val != ClipboardSelectionType.PhotoMode)
		{
			_eulerRotation = transform.localEulerAngles;
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

			transform.position += worldMove.normalized * _moveSpeed * Time.deltaTime;
		}
	}
	private void UpdateRotation()
	{
		// Only rotate when right mouse button is held
		if (!Input.GetMouseButton(1)) return;

		float mouseX = Input.GetAxisRaw("Mouse X");
		float mouseY = Input.GetAxisRaw("Mouse Y");
		var rotateAngles = new Vector3(-mouseY, mouseX, 0);
		if (rotateAngles.magnitude < 0.001f) return;

		float rotateAmount = (Time.deltaTime * _rotateSpeed);
		_eulerRotation += rotateAngles * rotateAmount;
		transform.localRotation = Quaternion.Euler(_eulerRotation);
	}
}

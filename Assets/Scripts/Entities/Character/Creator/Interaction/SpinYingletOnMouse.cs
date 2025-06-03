using Character.Creator.UI;
using UnityEngine;

public class SpinYingletOnMouse : MonoBehaviour
{
	[SerializeField] float _spinSensitivity = 10f;
	private IClipboardSelection _clipboardSelection;

	private void Awake()
	{
		_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
	}

	void Update()
	{
		// Early return if we're in photo mode
		if (_clipboardSelection.Selection.Val == ClipboardSelectionType.PhotoMode) return;

		if (Input.GetMouseButton(1))
		{
			float spinAmount = Input.GetAxisRaw("Mouse X") * _spinSensitivity;
			this.transform.rotation *= Quaternion.Euler(0, spinAmount, 0);
		}

	}
}

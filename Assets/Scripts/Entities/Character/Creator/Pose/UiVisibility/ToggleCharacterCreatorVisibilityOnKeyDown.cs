using UnityEngine;


namespace Character.Creator.UI
{
	public class ToggleCharacterCreatorVisibilityOnKeyDown : MonoBehaviour
	{
		private ICharacterCreatorVisibilityControl _visibilityControl;
		private IClipboardSelection _clipboardSelection;

		private void Awake()
		{
			_visibilityControl = this.GetComponent<ICharacterCreatorVisibilityControl>();
			_clipboardSelection = this.GetComponentInChildren<IClipboardSelection>();
		}

		private void Update()
		{
			// Only allow switching in pose mode, or if we somehow got to this state outside of it
			bool isPoseMode = _clipboardSelection.Selection.Val == ClipboardSelectionType.Pose;
			bool isVisible = _visibilityControl.IsVisible.Val;
			if (isPoseMode || !isVisible)
			{
				if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					_visibilityControl.Toggle();
				}
			}
		}
	}
}
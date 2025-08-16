using UnityEngine;


namespace Character.Creator.UI
{
	public class ToggleCharacterCreatorVisibilityOnKeyDown : MonoBehaviour
	{
		private ICharacterCreatorVisibilityControl _visibilityControl;
		private IInPoseModeChecker _inPoseMode;

		private void Awake()
		{
			_visibilityControl = this.GetComponent<ICharacterCreatorVisibilityControl>();
			_inPoseMode = this.GetComponentInChildren<IInPoseModeChecker>();
		}

		private void Update()
		{
			// Only allow switching in pose mode, or if we somehow got to this state outside of it
			bool isPoseMode = _inPoseMode.InPoseMode.Val;
			bool isVisible = _visibilityControl.IsVisible.Val;
			if (isPoseMode || !isVisible)
			{
				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					_visibilityControl.Toggle();
				}
			}
		}
	}
}
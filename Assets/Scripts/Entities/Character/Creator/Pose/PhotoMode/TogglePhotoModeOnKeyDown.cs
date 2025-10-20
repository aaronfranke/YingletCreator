using UnityEngine;


namespace Character.Creator.UI
{
	public class TogglePhotoModeOnKeyDown : MonoBehaviour
	{
		private IMenuManager _menuManager;
		private IPhotoModeState _photoModeState;
		private IInPoseModeChecker _inPoseMode;

		private void Awake()
		{
			_menuManager = Singletons.GetSingleton<IMenuManager>();
			_photoModeState = this.GetComponent<IPhotoModeState>();
			_inPoseMode = this.GetComponentInChildren<IInPoseModeChecker>();
		}

		private void Update()
		{
			if (_menuManager.OpenMenu.Val != null)
			{
				return; // If the user is looking at settings/about, don't let them navigate in
			}
			// Only allow switching in pose mode, or if we somehow got to this state outside of it
			bool isPoseMode = _inPoseMode.InPoseMode.Val;
			bool inPhotoMode = _photoModeState.IsInPhotoMode.Val;
			if (isPoseMode || inPhotoMode)
			{
				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					_photoModeState.Toggle();
				}
			}
		}
	}
}
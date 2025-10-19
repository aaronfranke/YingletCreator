using Character.Creator.UI;
using Reactivity;
using UnityEngine;

public class PoseCameraFOVReflector : ReactiveBehaviour
{
	private IInPoseModeChecker _inPoseMode;
	private IPoseCameraData _camData;
	private float _originalFOV;

	private void Start()
	{
		_inPoseMode = this.GetCharacterCreatorComponent<IInPoseModeChecker>();
		_camData = GetComponentInParent<IPoseCameraData>();
		_originalFOV = Camera.main.fieldOfView;
		AddReflector(Reflect);

	}
	void Reflect()
	{
		if (!_inPoseMode.InPoseMode.Val)
		{
			Camera.main.fieldOfView = _originalFOV;
		}
		Camera.main.fieldOfView = _camData.FieldOfView;
	}
}

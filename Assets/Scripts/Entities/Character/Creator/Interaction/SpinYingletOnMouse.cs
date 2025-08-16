using Character.Creator.UI;
using UnityEngine;

public class SpinYingletOnMouse : MonoBehaviour
{
	[SerializeField] float _spinSensitivity = 10f;
	private IInPoseModeChecker _inPoseMode;

	private void Awake()
	{
		_inPoseMode = this.GetCharacterCreatorComponent<IInPoseModeChecker>();
	}

	void Update()
	{
		// Early return if we're in photo mode
		if (_inPoseMode.InPoseMode.Val) return;

		if (Input.GetMouseButton(1))
		{
			float spinAmount = Input.GetAxisRaw("Mouse X") * _spinSensitivity;
			this.transform.rotation *= Quaternion.Euler(0, spinAmount, 0);
		}

	}
}

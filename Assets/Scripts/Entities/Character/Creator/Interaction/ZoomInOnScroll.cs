using Character.Creator.UI;
using UnityEngine;

public class ZoomInOnScroll : MonoBehaviour
{
	[SerializeField] Vector3 _zoomPos;
	[SerializeField] Vector3 _zoomRot;
	[SerializeField] float _scrollSensitivity = 1f;
	[SerializeField] float _lerpPower = 5f;

	IUiHoverManager _uiHoverManager;
	IClipboardSelection _clipboardSelection;
	IYingletHeightProvider _heightProvider;
	Vector3 _startPos;
	Quaternion _startRot;
	Quaternion _zoomRotQuaternion;
	float _targetPercent = 0f;
	float _currentPercent = 0;

	void Start()
	{
		_uiHoverManager = Singletons.GetSingleton<IUiHoverManager>();
		_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
		_heightProvider = this.GetCharacterCreatorComponent<IYingletHeightProvider>();
		_startPos = transform.localPosition;
		_startRot = transform.localRotation;
		_zoomRotQuaternion = Quaternion.Euler(_zoomRot);
	}

	void Update()
	{
		// Early return if we're in photo mode
		if (_clipboardSelection.Selection.Val == ClipboardSelectionType.Pose) return;

		UpdateTargetPercent();
		UpdatePos();
	}
	void UpdateTargetPercent()
	{
		// Early return if we're hovering over UI
		if (_uiHoverManager.HoveringUi) return;

		float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
		if (Mathf.Abs(scroll) > 0.0001f)
		{
			_targetPercent += scroll * _scrollSensitivity;
			_targetPercent = Mathf.Clamp01(_targetPercent);
		}
	}
	private void UpdatePos()
	{
		var zoomOffset = new Vector3(0, _heightProvider.YScale - 1, 0);
		_currentPercent = Mathf.Lerp(_currentPercent, _targetPercent, Time.deltaTime * _lerpPower);
		transform.localPosition = Vector3.Lerp(_startPos, _zoomPos + zoomOffset, _currentPercent);
		transform.localRotation = Quaternion.Lerp(_startRot, _zoomRotQuaternion, _currentPercent);
	}

}

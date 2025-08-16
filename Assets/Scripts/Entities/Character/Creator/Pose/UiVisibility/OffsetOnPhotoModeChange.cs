using Character.Creator.UI;
using UnityEngine;

public class OffsetOnPhotoModeChange : MonoBehaviour
{
	[SerializeField] SharedEaseSettings _easeSettings;
	[SerializeField] Vector3 _offset;

	private IPhotoModeState _photoModeState;
	private Vector3 _originalPos;
	private Coroutine _transitionCoroutine;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_photoModeState = this.GetComponentInParent<IPhotoModeState>();
		_originalPos = this.transform.localPosition;
		_photoModeState.IsInPhotoMode.OnChanged += OnPhotoModeChanged;
	}

	private void OnPhotoModeChanged(bool from, bool to)
	{
		var fromPos = this.transform.localPosition;
		var toPos = to ? _originalPos + _offset : _originalPos;
		this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => this.transform.localPosition = Vector3.LerpUnclamped(fromPos, toPos, p));
	}
}

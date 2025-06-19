using Character.Creator.UI;
using UnityEngine;

public class OffsetOnVisibilityChange : MonoBehaviour
{
	[SerializeField] SharedEaseSettings _easeSettings;
	[SerializeField] Vector3 _offset;

	private ICharacterCreatorVisibilityControl _visibilityControl;
	private Vector3 _originalPos;
	private Coroutine _transitionCoroutine;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
		_originalPos = this.transform.localPosition;
		_visibilityControl.IsVisible.OnChanged += OnVisibilityChanged;
	}

	private void OnVisibilityChanged(bool from, bool to)
	{
		var fromPos = this.transform.localPosition;
		var toPos = to ? _originalPos : _originalPos + _offset;
		this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => this.transform.localPosition = Vector3.LerpUnclamped(fromPos, toPos, p));
	}
}

using Character.Creator.UI;
using Reactivity;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DisableUIInputOnPhotoMode : ReactiveBehaviour
{
	private IPhotoModeState _photoModeState;
	private CanvasGroup _canvasGroup;

	private void Start()
	{
		_photoModeState = this.GetComponentInParent<IPhotoModeState>();
		_canvasGroup = this.GetComponent<CanvasGroup>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		bool inPhotoMode = _photoModeState.IsInPhotoMode.Val;
		_canvasGroup.interactable = !inPhotoMode;
	}
}

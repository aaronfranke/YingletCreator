
using Character.Creator.UI;
using Reactivity;
using UnityEngine;

public class DisableColliderOnPhotoMode : ReactiveBehaviour
{
	private IPhotoModeState _photoModeState;
	private Collider _collider;

	private void Start()
	{
		_photoModeState = this.GetComponentInParent<IPhotoModeState>();
		_collider = this.GetComponent<Collider>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		bool inPhotoMode = _photoModeState.IsInPhotoMode.Val;
		_collider.enabled = !inPhotoMode;
	}
}


using Character.Creator.UI;
using Reactivity;
using UnityEngine;

public class DisableColliderOnNoUi : ReactiveBehaviour
{
	private ICharacterCreatorVisibilityControl _visibilityControl;
	private Collider _collider;

	private void Start()
	{
		_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
		_collider = this.GetComponent<Collider>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		bool visible = _visibilityControl.IsVisible.Val;
		_collider.enabled = visible;
	}
}

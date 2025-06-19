using Character.Creator.UI;
using Reactivity;

public class SmartDisableOnNoUi : ReactiveBehaviour
{
	private ICharacterCreatorVisibilityControl _visibilityControl;
	private ISmartDisabler _smartDisabler;

	private void Start()
	{
		_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
		_smartDisabler = this.GetComponent<ISmartDisabler>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		bool visible = _visibilityControl.IsVisible.Val;
		_smartDisabler.SetActive(visible, this);
	}
}

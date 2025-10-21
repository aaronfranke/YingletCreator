using Reactivity;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class YingPortraitClicking : ReactiveBehaviour
	{
		private ICustomizationSelection _selection;
		private IYingPortraitReference _reference;
		private Button _button;

		private void Awake()
		{
			_selection = this.GetComponentInParent<ICustomizationSelection>();
			_reference = this.GetComponent<IYingPortraitReference>();
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

		}
		private void Start()
		{
			AddReflector(ReflectInteractable);
		}

		private new void OnDestroy()
		{
			base.OnDestroy();
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			_selection.SetSelected(_reference.Reference, withConfirmation: true);
		}
		void ReflectInteractable()
		{
			_button.interactable = !_reference.Selected.Val;
		}
	}
}

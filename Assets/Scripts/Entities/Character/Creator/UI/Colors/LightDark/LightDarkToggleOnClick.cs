using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class LightDarkToggleOnClick : MonoBehaviour
	{
		private Button _button;
		private ILightDarkSelection _selection;

		private void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

			_selection = this.GetComponentInParent<ILightDarkSelection>();
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			_selection.Toggle();
		}
	}
}
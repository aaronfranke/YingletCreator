using Reactivity;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggle : ReactiveBehaviour
	{
		private ICustomizationSelectedDataRepository _dataRepo;
		private ICharacterCreatorToggleReference _reference;
		private Toggle _toggle;

		private void Awake()
		{
			_reference = this.GetComponent<ICharacterCreatorToggleReference>();
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
			_toggle = this.GetComponentInChildren<Toggle>();
			_toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
		}
		private new void OnDestroy()
		{
			base.OnDestroy();
			_toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
		}

		private void Toggle_OnValueChanged(bool arg0)
		{
			_dataRepo.FlipToggle(_reference.ToggleId);
		}


		private void Start()
		{
			AddReflector(ReflectToggleValue);
		}


		private void ReflectToggleValue()
		{
			_toggle.SetIsOnWithoutNotify(_dataRepo.GetToggle(_reference.ToggleId));
		}


	}
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggleButton : MonoBehaviour, IUserToggleEvents
	{
		private ICustomizationSelectedDataRepository _dataRepo;
		private ICharacterCreatorToggleReference _reference;
		private Button _button;

		public event Action<bool> UserToggled;

		void Awake()
		{
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
			_reference = this.GetComponent<ICharacterCreatorToggleReference>();
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}


		private void Button_OnClick()
		{
			var from = _dataRepo.GetToggle(_reference.ToggleId);
			_dataRepo.FlipToggle(_reference.ToggleId);
			var to = _dataRepo.GetToggle(_reference.ToggleId);
			if (from != to)
			{
				UserToggled?.Invoke(to);
			}
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorTogglePoseButton : MonoBehaviour, IUserToggleEvents
	{
		private IPageYingPoseData _dataRepo;
		private ICharacterCreatorTogglePoseReference _reference;
		private Button _button;

		public event Action<bool> UserToggled;

		void Awake()
		{
			_dataRepo = this.GetComponentInParent<IPageYingPoseData>();
			_reference = this.GetComponent<ICharacterCreatorTogglePoseReference>();
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}


		private void Button_OnClick()
		{
			if (_dataRepo.Data == null) return;

			var from = _dataRepo.Data.Pose;
			var to = _reference.PoseId;
			if (from != to)
			{
				_dataRepo.Data.Pose = to;
				UserToggled?.Invoke(to);
			}
		}
	}
}

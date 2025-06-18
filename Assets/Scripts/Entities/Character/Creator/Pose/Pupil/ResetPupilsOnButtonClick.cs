using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ResetPupilsOnButtonClick : MonoBehaviour
	{
		private Button _button;
		private IPageYingPoseData _data;

		private void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

			_data = this.GetComponentInParent<IPageYingPoseData>();
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			var data = _data.Data;
			if (data == null) return;
			data.PupilData = PupilOffsetMutator_Default.InherentOffset;
		}
	}
}
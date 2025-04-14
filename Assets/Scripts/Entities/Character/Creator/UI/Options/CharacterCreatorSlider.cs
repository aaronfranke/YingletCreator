using Character.Data;
using Reactivity;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorSlider : ReactiveBehaviour
	{
		[SerializeField] CharacterSliderId _sliderId;
		private ICustomizationSelectedDataRepository _dataRepo;
		private Slider _slider;

		private void Awake()
		{
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
			_slider = this.GetComponentInChildren<Slider>();
			_slider.onValueChanged.AddListener(Slider_OnValueChanged);
		}

		private void Start()
		{
			AddReflector(ReflectSliderValue);
		}

		private new void OnDestroy()
		{
			base.OnDestroy();
			_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
		}

		private void ReflectSliderValue()
		{
			_slider.SetValueWithoutNotify(_dataRepo.GetSliderValue(_sliderId));
		}


		private void Slider_OnValueChanged(float arg0)
		{
			_dataRepo.SetSliderValue(_sliderId, arg0);
		}
	}
}
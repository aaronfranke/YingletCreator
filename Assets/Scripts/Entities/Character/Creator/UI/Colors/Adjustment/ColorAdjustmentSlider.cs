using Assets.Scripts.Entities.Character.Compositor;
using Reactivity;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ColorAdjustmentSlider : ReactiveBehaviour
	{
		private IColorActiveSelection _activeSelection;
		private ICustomizationSelectedDataRepository _dataRepo;
		private Slider _slider;

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
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
			var id = _activeSelection.FirstSelected;
			if (!id) return;
			_slider.SetValueWithoutNotify(_dataRepo.GetColorizeValues(id).Base.Hue);
		}


		private void Slider_OnValueChanged(float arg0)
		{
			var id = _activeSelection.FirstSelected;
			if (!id) return;

			var writeableColor = new WriteableColorizeValues(_dataRepo.GetColorizeValues(id));
			var diff = arg0 - writeableColor.Base.Hue;
			writeableColor.Base.Hue = arg0;
			writeableColor.Shade.Hue += diff;
			_dataRepo.SetColorizeValues(id, writeableColor);
		}
	}
}
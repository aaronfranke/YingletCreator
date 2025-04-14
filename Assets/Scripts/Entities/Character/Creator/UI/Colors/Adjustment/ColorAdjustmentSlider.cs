using Assets.Scripts.Entities.Character.Compositor;
using Character.Compositor;
using Character.Data;
using Reactivity;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ColorAdjustmentSlider : ReactiveBehaviour
	{
		[SerializeField] ColorAdjustmentSliderTarget _target;

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
			_slider.SetValueWithoutNotify(GetTargetVal(_dataRepo.GetColorizeValues(id).Base));
		}


		private void Slider_OnValueChanged(float value)
		{
			var ids = _activeSelection.AllSelected.ToList();
			if (!ids.Any()) return;

			var existingColors = ids.Select(id => new IdWithColor(id, _dataRepo.GetColorizeValues(id))).ToList();

			var firstColor = existingColors.First();
			var diff = value - GetTargetVal(firstColor.ColorizeValues.Base);

			using var suspender = new ReactivitySuspender();
			foreach (var color in existingColors)
			{
				var writeableColor = new WriteableColorizeValues(color.ColorizeValues);
				AdjustTargetVal(writeableColor.Base, diff);
				AdjustTargetVal(writeableColor.Shade, diff);
				_dataRepo.SetColorizeValues(color.Id, writeableColor);
			}
		}
		private sealed class IdWithColor
		{
			public IdWithColor(ReColorId id, IColorizeValues colorizeValues)
			{
				Id = id;
				ColorizeValues = colorizeValues;
			}
			public ReColorId Id { get; }
			public IColorizeValues ColorizeValues { get; }
		}

		float GetTargetVal(IColorizeValuesPart part)
		{
			if (_target == ColorAdjustmentSliderTarget.Hue) return part.Hue;
			else if (_target == ColorAdjustmentSliderTarget.Saturation) return part.Saturation;
			else if (_target == ColorAdjustmentSliderTarget.Value) return part.Value;
			else throw new ArgumentException("Invalid target");
		}

		void AdjustTargetVal(WriteableColorizeValuesPart part, float diff)
		{
			if (_target == ColorAdjustmentSliderTarget.Hue) part.Hue += diff;
			else if (_target == ColorAdjustmentSliderTarget.Saturation) part.Saturation += diff;
			else if (_target == ColorAdjustmentSliderTarget.Value) part.Value += diff;
			else throw new ArgumentException("Invalid target");
		}
	}

	public enum ColorAdjustmentSliderTarget
	{
		Hue,
		Saturation,
		Value
	}
}
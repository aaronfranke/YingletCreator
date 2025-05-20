using Character.Compositor;
using Character.Data;
using Reactivity;
using System;

namespace Character.Creator
{


	public sealed class ObservableCustomizationData
	{
		public Observable<string> Name { get; } = new();
		public ObservableCustomizationSliderData SliderData { get; }
		public ObservableCustomizationColorData ColorData { get; }
		public ObservableCustomizationToggleData ToggleData { get; }
		public ObservableCustomizationNumberData NumberData { get; }
		public DateTime CreationTime { get; }

		public ObservableCustomizationData(SerializableCustomizationData serializableData)
		{
			Name.Val = serializableData.Name;
			CreationTime = serializableData.CreationTime;
			SliderData = new(serializableData.SliderData);
			ColorData = new(serializableData.ColorData);
			ToggleData = new(serializableData.ToggleData);
			NumberData = new(serializableData.NumberData);
			CustomizationDataUpgradeUtils.UpgradeIfNeeded(this, serializableData.Version);
		}
	}

	public sealed class ObservableCustomizationSliderData
	{
		public ObservableCustomizationSliderData(SerializableCustomizationSliderData sliderData)
		{
			if (sliderData?.SliderValues == null) return;
			foreach (var sliderValue in sliderData.SliderValues)
			{
				var key = ResourceLoader.Load<CharacterSliderId>(sliderValue.Id);
				SliderValues[key] = new(sliderValue.Value);
			}
		}

		/// <summary>
		/// The float value is always in the range of 0 to 1
		/// </summary>
		public ObservableDict<CharacterSliderId, Observable<float>> SliderValues { get; } = new();
	}

	public sealed class ObservableCustomizationColorData
	{
		public ObservableCustomizationColorData(SerializableCustomizationColorData colorData)
		{
			if (colorData?.ColorizeValues == null) return;
			foreach (var colorizeValues in colorData.ColorizeValues)
			{
				var key = ResourceLoader.Load<ReColorId>(colorizeValues.Id);
				ColorizeValues[key] = new(colorizeValues.Values);
			}
		}

		public ObservableDict<ReColorId, Observable<IColorizeValues>> ColorizeValues { get; } = new();
	}
	public sealed class ObservableCustomizationToggleData
	{
		public ObservableCustomizationToggleData(SerializableCustomizationToggleData toggleData)
		{
			if (toggleData?.ToggleIds == null) return;
			foreach (var toggleIdString in toggleData.ToggleIds)
			{
				var toggleId = ResourceLoader.Load<CharacterToggleId>(toggleIdString);
				Toggles.Add(toggleId);
			}
		}

		public ObservableHashSet<CharacterToggleId> Toggles { get; } = new();
	}

	public sealed class ObservableCustomizationNumberData
	{
		public ObservableCustomizationNumberData(SerializableCustomizationNumberData numberData)
		{
			if (numberData?.IntValues == null) return;
			foreach (var sliderValue in numberData.IntValues)
			{
				var key = ResourceLoader.Load<CharacterIntId>(sliderValue.Id);
				IntValues[key] = new(sliderValue.Value);
			}
		}

		public ObservableDict<CharacterIntId, Observable<int>> IntValues { get; } = new();
	}
}

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
		public DateTime CreationTime { get; }

		//ICustomizationToggleData ToggleData { get; }
		//ICustomizationColorData ColorData { get; }
		// string Name { get; }
		// Author?
		// LastUpdateDate?

		public ObservableCustomizationData(SerializableCustomizationData serializableData)
		{
			Name.Val = serializableData.Name;
			CreationTime = serializableData.CreationTime;
			SliderData = new(serializableData.SliderData);
			ColorData = new(serializableData.ColorData);
			ToggleData = new(serializableData.ToggleData);
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
				SliderValues[key] = new Observable<float>(sliderValue.Value);
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
				ColorizeValues[key] = new Observable<IColorizeValues>(colorizeValues.Values);
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
}

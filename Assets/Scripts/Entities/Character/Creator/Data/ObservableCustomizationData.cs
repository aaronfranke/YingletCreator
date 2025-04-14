using Character.Compositor;
using Character.Data;
using Reactivity;

namespace Character.Creator
{


	public sealed class ObservableCustomizationData
	{
		public Observable<string> Name { get; } = new();
		public ObervableCustomizationSliderData SliderData { get; }
		public ObservableCustomizationColorData ColorData { get; }

		//ICustomizationToggleData ToggleData { get; }
		//ICustomizationColorData ColorData { get; }
		// string Name { get; }
		// Author?
		// LastUpdateDate?

		public ObservableCustomizationData(SerializableCustomizationData serializableData)
		{
			Name.Val = serializableData.Name;
			SliderData = new(serializableData.SliderData);
			ColorData = new();
		}
	}

	public sealed class ObervableCustomizationSliderData
	{
		public ObervableCustomizationSliderData(SerializableCustomizationSliderData sliderData)
		{
			if (sliderData?.SliderValues == null) return;
			foreach (var sliderValue in sliderData.SliderValues)
			{
				var key = ResourceLoader.Load<CharacterSliderId>(sliderValue.SliderID);
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
		public ObservableDict<ReColorId, Observable<IColorizeValues>> ColorizeValues { get; } = new();
	}




	// Below this line is old code planning that is likely no longer relevant

	//public interface ICustomizationToggleData
	//{
	//    /// <summary>
	//    /// What elements the user has toggled on
	//    /// Clicking a toggle may turn other toggles off (per configuration in the preset)
	//    /// </summary>
	//    IReadOnlyCollection<TogglePreset> EnabledToggles { get; }
	//}
	//public class TogglePreset : ScriptableObject
	//{
	//    // 
	//    // What tags to turn off
	//}

	//public interface ICustomizationColorData
	//{
	//    //IEnumerable<Color> Data;
	//}
	//public interface IColorAdjustmentData
	//{


	//    public sealed class SliderPreset : ScriptableObject
	//{
	//    /// <summary>
	//    /// Multiple targets, to control multiple bones
	//    /// Anything mirrored will probably have 2 of these (for left and right)
	//    /// These might
	//    /// - Scale bones
	//    /// - Shift bones
	//    /// - Even disable certain geometry altogether (i.e. removing boobs at 0)
	//    /// </summary>
	//    public IEnumerable<ISliderPresetTarget> Targets { get; }
	//}

	//public interface ISliderPresetTarget
	//{
	//    // 
	//    //public Vector3 MinScale { get; }
	//    //public Vector3  MaxScale { get; }
	//}
}

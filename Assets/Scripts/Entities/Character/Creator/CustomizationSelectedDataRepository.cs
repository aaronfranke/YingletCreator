using Character.Compositor;
using Character.Data;
using Reactivity;

namespace Character.Creator
{
	/// <summary>
	/// Returns observable data associated to the selected yinglet
	/// </summary>
	public interface ICustomizationSelectedDataRepository
	{
		ObservableCustomizationData CustomizationData { get; }
	}


	/// <summary>
	/// This is made available only for undo purposes; most consumers should not need to set this
	/// </summary>
	public interface IForceableCustomizationSelectedDataRepository : ICustomizationSelectedDataRepository
	{
		void ForceCustomizationData(SerializableCustomizationData cachedData);
	}

	public class CustomizationSelectedDataRepository : ReactiveBehaviour, IForceableCustomizationSelectedDataRepository
	{
		private ICustomizationSelection _selection;
		private Observable<ObservableCustomizationData> _data = new();

		public ObservableCustomizationData CustomizationData => _data.Val;

		void Awake()
		{
			_selection = this.GetComponent<ICustomizationSelection>();
			AddReflector(ReflectCustomizationData);
		}

		void ReflectCustomizationData()
		{
			// This used to be a computed, but with undo we want to be able to force the customization data to a specific state
			var cachedData = _selection.Selected.CachedData;
			_data.Val = new ObservableCustomizationData(cachedData);
		}

		public void ForceCustomizationData(SerializableCustomizationData cachedData)
		{
			_data.Val = new ObservableCustomizationData(cachedData);
		}
	}

	public static class CharacterCreatorDataRepositoryExtensionMethods
	{
		public static float GetSliderValue(this ICustomizationSelectedDataRepository dataRepo, CharacterSliderId id)
		{

			if (dataRepo.CustomizationData.SliderData.SliderValues.TryGetValue(id, out Observable<float> value))
			{
				return value.Val;
			}
			return 0.5f;
		}
		public static void SetSliderValue(this ICustomizationSelectedDataRepository dataRepo, CharacterSliderId id, float value)
		{
			ObservableDictUtils<CharacterSliderId, float>.SetOrUpdate(dataRepo.CustomizationData.SliderData.SliderValues, id, value);
		}
		public static IColorizeValues GetColorizeValues(this ICustomizationSelectedDataRepository dataRepository, ReColorId id)
		{
			if (dataRepository.CustomizationData.ColorData.ColorizeValues.TryGetValue(id, out Observable<IColorizeValues> values))
			{
				return values.Val;
			}
			return id.ColorGroup.DefaultColors;
		}
		public static void SetColorizeValues(this ICustomizationSelectedDataRepository dataRepo, ReColorId id, IColorizeValues values)
		{
			ObservableDictUtils<ReColorId, IColorizeValues>.SetOrUpdate(dataRepo.CustomizationData.ColorData.ColorizeValues, id, values);
		}

		public static bool GetToggle(this ICustomizationSelectedDataRepository dataRepo, CharacterToggleId id)
		{
			return dataRepo.CustomizationData.ToggleData.GetToggle(id);
		}
		public static void FlipToggle(this ICustomizationSelectedDataRepository dataRepo, CharacterToggleId id)
		{
			dataRepo.CustomizationData.ToggleData.FlipToggle(id);
		}

		public static int GetInt(this ICustomizationSelectedDataRepository dataRepo, CharacterIntId id)
		{
			return dataRepo.CustomizationData.NumberData.GetInt(id);
		}
		public static void SetInt(this ICustomizationSelectedDataRepository dataRepo, CharacterIntId id, int value)
		{
			ObservableDictUtils<CharacterIntId, int>.SetOrUpdate(dataRepo.CustomizationData.NumberData.IntValues, id, value);
		}
	}
}
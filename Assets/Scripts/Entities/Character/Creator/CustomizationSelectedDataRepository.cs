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

	public class CustomizationSelectedDataRepository : ReactiveBehaviour, ICustomizationSelectedDataRepository
	{
		private ICustomizationSelection _selection;
		private Computed<ObservableCustomizationData> _data;

		public ObservableCustomizationData CustomizationData => _data.Val;

		void Awake()
		{
			_selection = this.GetComponent<ICustomizationSelection>();
			_data = CreateComputed(ComputeCustomizationData);
		}

		ObservableCustomizationData ComputeCustomizationData()
		{
			var cachedData = _selection.Selected.CachedData;
			return new ObservableCustomizationData(cachedData);
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
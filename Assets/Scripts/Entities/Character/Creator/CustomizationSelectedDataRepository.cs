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
        public static float GetSliderValue(this ICustomizationSelectedDataRepository dataRepository, CharacterSliderId sliderId)
        {

            if (dataRepository.CustomizationData.SliderData.SliderValues.TryGetValue(sliderId, out Observable<float> value))
            {
                return value.Val;
            }
            return 0.5f;
        }
    }

}
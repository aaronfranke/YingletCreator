using Character.Data;
using UnityEngine;

namespace Character.Creator
{
    /// <summary>
    /// Returns observable data associated to the selected yinglet
    /// </summary>
    public interface ICustomizationSelectedDataRepository
    {
        ICustomizationData CustomizationData { get; }
    }

    public class CustomizationSelectedDataRepository : MonoBehaviour, ICustomizationSelectedDataRepository
    {
        ObservableCustomizationData _customizationData = new ObservableCustomizationData();

        public ICustomizationData CustomizationData => _customizationData;
    }

    public static class CharacterCreatorDataRepositoryExtensionMethods
    {
        public static float GetSliderValue(this ICustomizationSelectedDataRepository dataRepository, CharacterSliderId sliderId)
        {

            if (dataRepository.CustomizationData.SliderData.SliderValues.TryGetValue(sliderId, out float value))
            {
                return value;
            }
            return 0.5f;
        }
    }

}
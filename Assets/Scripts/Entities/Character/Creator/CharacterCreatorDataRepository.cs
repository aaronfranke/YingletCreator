using Character.Data;
using UnityEngine;

namespace Character.Creator
{

    public interface ICharacterCreatorDataRepository
    {
        ICustomizationData CustomizationData { get; }
    }

    public class CharacterCreatorDataRepository : MonoBehaviour, ICharacterCreatorDataRepository
    {
        ObservableCustomizationData _customizationData = new ObservableCustomizationData();

        public ICustomizationData CustomizationData => _customizationData;
    }

    public static class CharacterCreatorDataRepositoryExtensionMethods
    {
        public static float GetSliderValue(this ICharacterCreatorDataRepository dataRepository, CharacterSliderId sliderId)
        {

            if (dataRepository.CustomizationData.SliderData.SliderValues.TryGetValue(sliderId, out float value))
            {
                return value;
            }
            return 0.5f;
        }
    }

}
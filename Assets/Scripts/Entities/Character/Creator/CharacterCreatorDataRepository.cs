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

}
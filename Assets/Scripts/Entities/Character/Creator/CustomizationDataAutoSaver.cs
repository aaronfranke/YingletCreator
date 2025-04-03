using UnityEngine;

namespace Character.Creator
{
    public class CustomizationDataAutoSaver : MonoBehaviour
    {
        private ICustomizationDataSaver _dataSaver;
        private ICharacterCreatorDataRepository _dataRepository;

        private void Awake()
        {
            _dataSaver = this.GetComponent<ICustomizationDataSaver>();
            _dataRepository = this.GetComponent<ICharacterCreatorDataRepository>();
        }

        void Update()
        {
            // Manual saving too
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                _dataSaver.Save(_dataRepository.CustomizationData);
            }
        }
    }
}

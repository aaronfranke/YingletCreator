using Character.Creator;
using TMPro;
using UnityEngine;

public class NameTextField : MonoBehaviour
{
    private ICharacterCreatorDataRepository _dataRepository;
    private TMP_InputField _inputField;

    private void Awake()
    {
        _dataRepository = this.GetComponentInParent<ICharacterCreatorDataRepository>();
        _inputField = this.GetComponent<TMP_InputField>();
        _inputField.onValueChanged.AddListener(InputField_OnValueChanged);
    }

    private void OnDestroy()
    {
        _inputField.onValueChanged.RemoveListener(InputField_OnValueChanged);
    }

    private void InputField_OnValueChanged(string arg0)
    {
        _dataRepository.CustomizationData.Name = arg0;
    }
}

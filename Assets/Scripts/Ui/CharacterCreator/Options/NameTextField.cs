using Character.Creator;
using Reactivity;
using TMPro;

public class NameTextField : ReactiveBehaviour
{
    private ICustomizationSelectedDataRepository _dataRepository;
    private TMP_InputField _inputField;

    private void Awake()
    {
        _dataRepository = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
        _inputField = this.GetComponent<TMP_InputField>();
        _inputField.onValueChanged.AddListener(InputField_OnValueChanged);
    }

    private void Start()
    {
        AddReflector(ReflectText);
    }

    void ReflectText()
    {
        _inputField.text = _dataRepository.CustomizationData.Name.Val;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        _inputField.onValueChanged.RemoveListener(InputField_OnValueChanged);
    }

    private void InputField_OnValueChanged(string arg0)
    {
        _dataRepository.CustomizationData.Name.Val = arg0;
    }
}

using Character.Creator;
using UnityEngine;
using UnityEngine.UI;

public class SaveOnButtonClick : MonoBehaviour
{
    private Button _button;
    private ICustomizationDiskIO _dataSaver;

    private void Awake()
    {
        _button = this.GetComponent<Button>();
        _button.onClick.AddListener(Button_OnClick);

        _dataSaver = this.GetComponentInParent<ICustomizationDiskIO>();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Button_OnClick);
    }

    private void Button_OnClick()
    {
        _dataSaver.SaveSelected();
    }
}

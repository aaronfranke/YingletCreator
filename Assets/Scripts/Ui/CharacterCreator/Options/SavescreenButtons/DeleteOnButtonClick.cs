using Character.Creator;
using UnityEngine;
using UnityEngine.UI;

public class DeleteOnButtonClick : MonoBehaviour
{
    private Button _button;
    private ICustomizationDiskIO _diskIO;

    private void Awake()
    {
        _button = this.GetComponent<Button>();
        _button.onClick.AddListener(Button_OnClick);

        _diskIO = this.GetComponentInParent<ICustomizationDiskIO>();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Button_OnClick);
    }

    private void Button_OnClick()
    {
        _diskIO.DeleteSelected();
    }
}

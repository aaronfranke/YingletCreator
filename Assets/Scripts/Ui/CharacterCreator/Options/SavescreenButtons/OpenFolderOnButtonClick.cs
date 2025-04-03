using Character.Creator;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class OpenFolderOnButtonClick : MonoBehaviour
{
    private Button _button;
    private ICustomizationDataSaver _dataSaver;

    private void Awake()
    {
        _button = this.GetComponent<Button>();
        _button.onClick.AddListener(Button_OnClick);

        _dataSaver = this.GetComponentInParent<ICustomizationDataSaver>();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Button_OnClick);
    }

    private void Button_OnClick()
    {
        Process.Start("explorer.exe", _dataSaver.FolderRoot);
    }
}

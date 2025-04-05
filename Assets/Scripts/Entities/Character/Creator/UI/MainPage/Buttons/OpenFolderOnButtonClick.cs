using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


namespace Character.Creator.UI
{
    public class OpenFolderOnButtonClick : MonoBehaviour
    {
        private Button _button;
        private ICustomizationSaveFolderProvider _folderProvider;

        private void Awake()
        {
            _button = this.GetComponent<Button>();
            _button.onClick.AddListener(Button_OnClick);

            _folderProvider = this.GetComponentInParent<ICustomizationSaveFolderProvider>();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            Process.Start("explorer.exe", _folderProvider.CustomFolderRoot);
        }
    }
}
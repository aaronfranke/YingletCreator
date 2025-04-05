using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
    public class NewYingOnButtonClick : MonoBehaviour
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
            _diskIO.DuplicateSelected();
        }
    }
}
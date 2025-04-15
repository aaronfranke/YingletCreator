using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ButtonCopyPasteColor : MonoBehaviour
	{
		[SerializeField] bool _copy;
		private Button _button;
		private IColorCopyPasting _copyPasting;

		private void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

			_copyPasting = this.GetComponentInParent<IColorCopyPasting>();
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			if (_copy)
			{
				_copyPasting.Copy();
			}
			else
			{
				_copyPasting.Paste();
			}
		}
	}
}
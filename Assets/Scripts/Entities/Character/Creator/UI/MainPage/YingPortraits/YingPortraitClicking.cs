using Reactivity;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class YingPortraitClicking : ReactiveBehaviour
	{
		private ICustomizationSelection _selection;
		private ICharacterCreatorUndoManager _undoManager;
		private IYingPortraitReference _reference;
		private Button _button;

		private void Awake()
		{
			_selection = this.GetComponentInParent<ICustomizationSelection>();
			_undoManager = this.GetComponentInParent<ICharacterCreatorUndoManager>();
			_reference = this.GetComponent<IYingPortraitReference>();
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

		}
		private void Start()
		{
			AddReflector(ReflectInteractable);
		}

		private new void OnDestroy()
		{
			base.OnDestroy();
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			_undoManager.RecordState($"Selected yinglet: {_reference.Reference.CachedData.Name}");
			_selection.Selected = _reference.Reference;
		}
		void ReflectInteractable()
		{
			_button.interactable = !_reference.Selected.Val;
		}
	}
}

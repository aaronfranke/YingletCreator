using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class DeleteOnButtonClick : MonoBehaviour
	{
		private Button _button;
		private ICustomizationDiskIO _diskIO;
		private ICharacterCreatorUndoManager _undoManager;
		private IConfirmationManager _confirmationManager;

		private void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

			_diskIO = this.GetComponentInParent<ICustomizationDiskIO>();
			_undoManager = this.GetComponentInParent<ICharacterCreatorUndoManager>();
			_confirmationManager = Singletons.GetSingleton<IConfirmationManager>();
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			_confirmationManager.OpenConfirmation(new(
				"Are you sure you want to delete\nthis yinglet?\n\nThis action is irreversible.",
				"Delete Yinglet",
				"delete-yinglet",
				ExecuteDelete));
		}

		void ExecuteDelete()
		{
			_undoManager.RecordState("Deleted yinglet");
			_diskIO.DeleteSelected();
		}
	}
}
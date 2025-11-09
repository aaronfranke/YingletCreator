using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


namespace Character.Creator.UI
{
	public enum OpenFolderOnClickType
	{
		Custom,
		Photos
	}

	public class OpenFolderOnButtonClick : MonoBehaviour
	{
		[SerializeField] private OpenFolderOnClickType _type;

		private Button _button;
		private ICharacterCreatorFolderProvider _folderProvider;

		private void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(Button_OnClick);

			_folderProvider = this.GetComponentInParent<ICharacterCreatorFolderProvider>();
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			var folder = _type switch
			{
				OpenFolderOnClickType.Custom => _folderProvider.CustomFolderRoot,
				OpenFolderOnClickType.Photos => _folderProvider.PhotoRoot,
				_ => _folderProvider.CustomFolderRoot
			};
			Process.Start("explorer.exe", folder);
		}
	}
}
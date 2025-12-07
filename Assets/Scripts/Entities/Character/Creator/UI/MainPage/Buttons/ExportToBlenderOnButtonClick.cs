using System;
using UnityEngine;
using UnityEngine.UI;

public class ExportToBlenderOnButtonClick : MonoBehaviour
{
	private Button _button;
	private IConfirmationManager _confirmationManager;

	public event Action OnExport = delegate { };

	private void Awake()
	{
		_button = this.GetComponent<Button>();
		_button.onClick.AddListener(Button_OnClick);

		_confirmationManager = Singletons.GetSingleton<IConfirmationManager>();
	}

	private void OnDestroy()
	{
		_button.onClick.RemoveListener(Button_OnClick);
	}

	private void Button_OnClick()
	{
		_confirmationManager.OpenConfirmation(new(
			"Export model + textures to Blender?\nThis feature has limited support and\nrequires additional work.",
			"Export",
			"blender-export",
			ExecuteExport));
	}

	void ExecuteExport()
	{
		OnExport();
	}
}

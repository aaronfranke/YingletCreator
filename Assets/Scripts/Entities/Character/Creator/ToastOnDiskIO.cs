using Character.Creator;
using UnityEngine;

public class ToastOnDiskIO : MonoBehaviour
{
	private IToastDisplay _toastDisplay;
	private ICustomizationDiskIO _diskIO;

	void Start()
	{
		_toastDisplay = Singletons.GetSingleton<IToastDisplay>();
		_diskIO = this.GetComponent<ICustomizationDiskIO>();

		_diskIO.OnSaved += DiskIO_OnSaved;
		_diskIO.OnDeleted += DiskIO_OnDeleted;
	}

	private void OnDestroy()
	{
		_diskIO.OnSaved -= DiskIO_OnSaved;
		_diskIO.OnDeleted -= DiskIO_OnDeleted;
	}

	private void DiskIO_OnSaved(string fileName)
	{
		_toastDisplay.Show($"Saved {fileName}");
	}

	private void DiskIO_OnDeleted(string fileName)
	{
		_toastDisplay.Show($"Deleted {fileName}");
	}
}

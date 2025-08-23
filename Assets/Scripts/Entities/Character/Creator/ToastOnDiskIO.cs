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

	}

	private void DiskIO_OnSaved()
	{
		_toastDisplay.Show("Yinglet saved as (insert path name here)");
	}
}

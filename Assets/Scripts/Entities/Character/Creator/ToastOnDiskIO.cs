using Character.Creator;
using UnityEngine;

public class ToastOnDiskIO : MonoBehaviour
{
	private IToastMediator _toastMediator;
	private ICustomizationDiskIO _diskIO;

	void Start()
	{
		_toastMediator = Singletons.GetSingleton<IToastMediator>();
		_diskIO = this.GetComponent<ICustomizationDiskIO>();

		_diskIO.OnSaved += DiskIO_OnSaved;

	}

	private void DiskIO_OnSaved()
	{
		_toastMediator.ShowToast("Yinglet saved as (insert path name here)");
	}
}

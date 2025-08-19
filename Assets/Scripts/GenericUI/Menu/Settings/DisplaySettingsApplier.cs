using Reactivity;
using UnityEngine;

public class DisplaySettingsApplier : ReactiveBehaviour
{
	private ISettingsManager _settingsManager;

	void Start()
	{
		_settingsManager = Singletons.GetSingleton<ISettingsManager>();
		AddReflector(ReflectFpsCap);
		AddReflector(ReflectScreenResolution);
	}

	private void ReflectFpsCap()
	{
		int fpsCap = (int)_settingsManager.Settings.FpsCap;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = fpsCap;
	}

	private void ReflectScreenResolution()
	{
		Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
	}
}

using Reactivity;
using UnityEngine;

public class DisplaySettingsApplier : ReactiveBehaviour
{
    private ISettingsManager _settingsManager;

    void Start()
    {
        _settingsManager = Singletons.GetSingleton<ISettingsManager>();
        AddReflector(ReflectFpsCap);
        AddReflector(ReflectWindowSize);
        AddReflector(ReflectScreenMode);
    }

    private void ReflectFpsCap()
    {
        int fpsCap = (int)_settingsManager.Settings.FpsCap;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsCap;
    }

    private void ReflectScreenMode()
    {
        switch (_settingsManager.Settings.ScreenMode)
        {
            case ScreenMode.Windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case ScreenMode.Borderless:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Fullscreen:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
    }

    private void ReflectWindowSize()
    {
        var size = _settingsManager.Settings.DisplayResolution;
        Screen.SetResolution(size.x, size.y, Screen.fullScreenMode);
    }
}

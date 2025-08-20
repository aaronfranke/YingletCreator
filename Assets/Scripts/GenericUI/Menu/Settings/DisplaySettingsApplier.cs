using Reactivity;
using System;
using UnityEngine;

public class DisplaySettingsApplier : ReactiveBehaviour
{
	private ISettingsManager _settingsManager;

	void Start()
	{
		_settingsManager = Singletons.GetSingleton<ISettingsManager>();
		AddReflector(ReflectFpsCap);
		AddReflector(ReflectScreenMode);
		AddReflector(ReflectWindowSize);
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
		var size = GetResolutionForWindowSize(_settingsManager.Settings.WindowSize);
		Screen.SetResolution(size.x, size.y, Screen.fullScreenMode);
	}

	private Vector2Int GetResolutionForWindowSize(WindowSize windowSize)
	{
		return windowSize switch
		{
			WindowSize.Resolution1024x768 => new(1024, 768),
			WindowSize.Resolution1280x720 => new(1280, 720),
			WindowSize.Resolution1280x800 => new(1280, 800),
			WindowSize.Resolution1280x960 => new(1280, 960),
			WindowSize.Resolution1440x900 => new(1440, 900),
			WindowSize.Resolution1600x900 => new(1600, 900),
			WindowSize.Resolution1680x1050 => new(1680, 1050),
			WindowSize.Resolution1920x1080 => new(1920, 1080),
			WindowSize.Resolution1920x1200 => new(1920, 1200),
			WindowSize.Resolution2560x1080 => new(2560, 1080),
			WindowSize.Resolution2560x1440 => new(2560, 1440),
			WindowSize.Resolution3440x1440 => new(3440, 1440),
			WindowSize.Resolution3840x2160 => new(3840, 2160),
			_ => throw new ArgumentException()
		};
	}
}

using UnityEngine;

[System.Serializable]
public sealed class SerializableSettings : ISettings
{
	[SerializeField] FpsCap _fpsCap = FpsCap.Fps120;
	public FpsCap FpsCap { get => _fpsCap; }

	[SerializeField] ScreenMode _screenMode = ScreenMode.Borderless;
	public ScreenMode ScreenMode { get => _screenMode; }

	[SerializeField] WindowSize _windowSize = WindowSize.Resolution1920x1080;
	public WindowSize WindowSize { get => _windowSize; }

	[SerializeField] float _effectVolume = 1.0f;
	public float EffectVolume => _effectVolume;

	[SerializeField] float _musicVolume = 0.7f;
	public float MusicVolume => _musicVolume;

	public SerializableSettings() { }

	public SerializableSettings(ISettings settings)
	{
		_fpsCap = settings.FpsCap;
		_screenMode = settings.ScreenMode;
		_windowSize = settings.WindowSize;
		_effectVolume = settings.EffectVolume;
		_musicVolume = settings.MusicVolume;
	}
}

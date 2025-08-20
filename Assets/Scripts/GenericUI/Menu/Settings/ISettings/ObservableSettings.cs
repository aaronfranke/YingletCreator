
using Reactivity;

public sealed class ObservableSettings : IWriteableSettings
{
	Observable<FpsCap> _fpsCap;
	public FpsCap FpsCap { get => _fpsCap.Val; set => _fpsCap.Val = value; }

	Observable<ScreenMode> _screenMode;
	public ScreenMode ScreenMode { get => _screenMode.Val; set => _screenMode.Val = value; }

	Observable<WindowSize> _windowSize;
	public WindowSize WindowSize { get => _windowSize.Val; set => _windowSize.Val = value; }

	Observable<float> _effectVolume;
	public float EffectVolume { get => _effectVolume.Val; set => _effectVolume.Val = value; }

	Observable<float> _musicVolume;
	public float MusicVolume { get => _musicVolume.Val; set => _musicVolume.Val = value; }

	public ObservableSettings(ISettings loadedSettings)
	{
		_fpsCap = new Observable<FpsCap>(loadedSettings.FpsCap);
		_screenMode = new Observable<ScreenMode>(loadedSettings.ScreenMode);
		_windowSize = new Observable<WindowSize>(loadedSettings.WindowSize);
		_effectVolume = new Observable<float>(loadedSettings.EffectVolume);
		_musicVolume = new Observable<float>(loadedSettings.MusicVolume);
	}
}

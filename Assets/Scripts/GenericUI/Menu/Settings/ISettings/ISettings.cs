public enum FpsCap
{
	Fps30 = 30,
	Fps60 = 60,
	Fps120 = 120,
	Fps144 = 144,
	Fps240 = 240,
	Unlimited = -1,
}

public enum ScreenMode
{
	Windowed,
	Borderless,
	Fullscreen
}

public enum WindowSize
{
	Resolution1024x768,
	Resolution1280x720,
	Resolution1280x800,
	Resolution1280x960,
	Resolution1440x900,
	Resolution1600x900,
	Resolution1680x1050,
	Resolution1920x1080,
	Resolution1920x1200,
	Resolution2560x1080,
	Resolution2560x1440,
	Resolution3440x1440,
	Resolution3840x2160
}

public interface ISettings
{
	FpsCap FpsCap { get; }
	ScreenMode ScreenMode { get; }
	WindowSize WindowSize { get; }

	float EffectVolume { get; }
	float MusicVolume { get; }
}

public interface IWriteableSettings : ISettings
{
	new FpsCap FpsCap { get; set; }
	new ScreenMode ScreenMode { get; set; }
	new WindowSize WindowSize { get; set; }
	new float EffectVolume { get; set; }
	new float MusicVolume { get; set; }
}

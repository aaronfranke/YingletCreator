using UnityEngine;

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

public interface ISettings
{
    FpsCap FpsCap { get; }
    ScreenMode ScreenMode { get; }
    Vector2Int DisplayResolution { get; }

    float EffectVolume { get; }
    float MusicVolume { get; }
}

public interface IWriteableSettings : ISettings
{
    new FpsCap FpsCap { get; set; }
    new ScreenMode ScreenMode { get; set; }
    new Vector2Int DisplayResolution { get; set; }
    new float EffectVolume { get; set; }
    new float MusicVolume { get; set; }
}

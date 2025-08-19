public enum FpsCap
{
	Fps30 = 30,
	Fps60 = 60,
	Fps120 = 120,
	Fps144 = 144,
	Fps240 = 240,
	Unlimited = -1,
}

public interface ISettings
{
	FpsCap FpsCap { get; }

	float EffectVolume { get; }
}

public interface IWriteableSettings : ISettings
{
	new FpsCap FpsCap { get; set; }
	new float EffectVolume { get; set; }
}

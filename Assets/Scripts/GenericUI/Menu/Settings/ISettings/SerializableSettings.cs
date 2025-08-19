[System.Serializable]
public sealed class SerializableSettings : ISettings
{
	FpsCap _fpsCap = FpsCap.Fps120;
	public FpsCap FpsCap { get => _fpsCap; }

	float _effectVolume = 1.0f;
	public float EffectVolume => _effectVolume;

	public SerializableSettings() { }

	public SerializableSettings(ISettings settings)
	{
		_fpsCap = settings.FpsCap;
		_effectVolume = settings.EffectVolume;
	}
}

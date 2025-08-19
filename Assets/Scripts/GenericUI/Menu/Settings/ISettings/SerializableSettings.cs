[System.Serializable]
public sealed class SerializableSettings : ISettings
{
	FpsCap _fpsCap = FpsCap.Fps120;

	public FpsCap FpsCap { get => _fpsCap; }
}

using UnityEngine;

[System.Serializable]
public sealed class SerializableSettings : ISettings
{
	[SerializeField] FpsCap _fpsCap = FpsCap.Fps120;
	public FpsCap FpsCap { get => _fpsCap; }

	[SerializeField] float _effectVolume = 1.0f;
	public float EffectVolume => _effectVolume;

	[SerializeField] float _musicVolume = 0.7f;
	public float MusicVolume => _musicVolume;

	public SerializableSettings() { }

	public SerializableSettings(ISettings settings)
	{
		_fpsCap = settings.FpsCap;
		_effectVolume = settings.EffectVolume;
		_musicVolume = settings.MusicVolume;
	}
}

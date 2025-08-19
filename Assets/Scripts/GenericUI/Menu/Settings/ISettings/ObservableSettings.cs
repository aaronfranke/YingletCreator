
using Reactivity;

public sealed class ObservableSettings : IWriteableSettings
{
	Observable<FpsCap> _fpsCap;
	public FpsCap FpsCap { get => _fpsCap.Val; set => _fpsCap.Val = value; }

	Observable<float> _effectVolume;
	public float EffectVolume { get => _effectVolume.Val; set => _effectVolume.Val = value; }

	public ObservableSettings(ISettings loadedSettings)
	{
		_fpsCap = new Observable<FpsCap>(loadedSettings.FpsCap);
		_effectVolume = new Observable<float>(loadedSettings.EffectVolume);
	}
}

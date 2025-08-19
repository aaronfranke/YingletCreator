
using Reactivity;

public sealed class ObservableSettings : IWriteableSettings
{
	Observable<FpsCap> _fpsCap;
	public FpsCap FpsCap { get => _fpsCap.Val; set => _fpsCap.Val = value; }

	public ObservableSettings(ISettings loadedSettings)
	{
		_fpsCap = new Observable<FpsCap>(loadedSettings.FpsCap);
	}
}

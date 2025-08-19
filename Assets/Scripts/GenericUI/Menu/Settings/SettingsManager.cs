using Reactivity;
using UnityEngine;

public interface ISettingsManager
{
	IWriteableSettings Settings { get; }
}
public class SettingsManager : MonoBehaviour, ISettingsManager
{
	private ISettingsIO _settingsIO;

	Observable<IWriteableSettings> _settings = new();

	public IWriteableSettings Settings => _settings.Val;

	private void Awake()
	{
		_settingsIO = Singletons.GetSingleton<ISettingsIO>();
		var loadedSettings = _settingsIO.LoadSettings();
		_settings.Val = new ObservableSettings(loadedSettings);
	}
}

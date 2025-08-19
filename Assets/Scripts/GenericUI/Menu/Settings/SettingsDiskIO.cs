
using System.IO;
using UnityEngine;

public interface ISettingsDiskIO
{
	ISettings LoadSettings();
	void SaveSettings(ISettings settings);
}
public class SettingsDiskIO : MonoBehaviour, ISettingsDiskIO
{
	private ISaveFolderProvider _saveFolderProvider;
	private string _settingsFilePath;

	private void Awake()
	{
		_saveFolderProvider = Singletons.GetSingleton<ISaveFolderProvider>();
		_settingsFilePath = Path.Combine(_saveFolderProvider.GameRootFolderPath, "settings.json");
	}

	public ISettings LoadSettings()
	{
		return new SerializableSettings();
	}

	public void SaveSettings(ISettings settings)
	{
		var serialized = new SerializableSettings(settings);
		string json = JsonUtility.ToJson(serialized, true);
		File.WriteAllText(_settingsFilePath, json);
	}
}

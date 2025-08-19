
using UnityEngine;

public interface ISettingsIO
{
	ISettings LoadSettings();
}
internal class SettingsIO : MonoBehaviour, ISettingsIO
{
	public ISettings LoadSettings()
	{
		return new SerializableSettings();
	}
}

using Character.Creator;
using System.IO;
using UnityEngine;


public class StaticDataRepository : MonoBehaviour, ICustomizationSelectedDataRepository
{
	[SerializeField] string _pathToYing;
	public ObservableCustomizationData CustomizationData { get; private set; }

	void Awake()
	{
		string text = File.ReadAllText(_pathToYing);
		var serializedData = JsonUtility.FromJson<SerializableCustomizationData>(text);
		CustomizationData = new(serializedData);
		Debug.Log("set the data");
	}
}

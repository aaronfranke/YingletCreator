using Character.Creator;
using System.IO;
using UnityEngine;


public class StaticDataRepository : MonoBehaviour, ICustomizationSelectedDataRepository
{
	[SerializeField] string _pathToYing;
	public ObservableCustomizationData CustomizationData { get; private set; }

	void Awake()
	{
		var resourceLoader = Singletons.GetSingleton<ICompositeResourceLoader>();
		string text = File.ReadAllText(_pathToYing);
		var serializedData = JsonUtility.FromJson<SerializableCustomizationData>(text);
		CustomizationData = new(serializedData, resourceLoader);
	}
}

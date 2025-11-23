using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;


internal sealed class ResourceProvider_TableLookup : MonoBehaviour, IResourceProvider
{
	private Dictionary<string, Object> _dictionary = new();

	public void Setup()
	{
		var modProvider = Singletons.GetSingleton<IModLoader>();
		foreach (var mod in modProvider.AllMods)
		{
			AddTableToDictionary(mod.Table);
		}
	}

	void AddTableToDictionary(ResourceLookupTable table)
	{
		foreach (var pair in table.Resources)
		{
			if (!_dictionary.TryAdd(pair.Guid, pair.Object))
			{
				// This GUID already exists
				// Let's force it, giving modders the potential opportunity to override base assets by moving them into their mod folder
				// I'll consider this behavior undefined / unsupported though
				_dictionary[pair.Guid] = pair.Object;
				Debug.LogWarning($"Adding object with guid {pair.Guid} and name {pair.Object.name} despite it already existing");
			}
		}
	}

	public T Load<T>(string guid) where T : Object
	{
		if (string.IsNullOrEmpty(guid))
		{
			return null;
		}

		bool loaded = _dictionary.TryGetValue(guid, out Object obj);
		if (!loaded)
		{
			Debug.LogError($"Couldn't find guid {guid} of type {typeof(T).Name} in lookup table; returning null");
			return null;
		}
		var typedObj = obj as T;
		if (obj is null)
		{
			Debug.LogError($"Attempted to cast guid {guid} to type {typeof(T).Name} but failed");
			return null;
		}
		return typedObj;
	}

	public IEnumerable<T> LoadAll<T>() where T : Object
	{
		return _dictionary
			.Values
			.Select(v => v as T)
			.Where(v => v != null)
			.ToArray();
	}

}

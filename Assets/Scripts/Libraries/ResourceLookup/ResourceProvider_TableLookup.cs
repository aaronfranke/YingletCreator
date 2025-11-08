using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;


internal sealed class ResourceProvider_TableLookup : MonoBehaviour, IResourceProvider
{
	[SerializeField] DefaultResourceLookupTable _defaultResourceTable;
	private Dictionary<string, Object> _dictionary;

	public void Setup()
	{
		_dictionary = _defaultResourceTable.Table.Resources.ToDictionary(kvp => kvp.Guid, kvp => kvp.Object);
		_defaultResourceTable = null; // Don't need this hogging memory any more
	}

	public T Load<T>(string guid) where T : Object
	{
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

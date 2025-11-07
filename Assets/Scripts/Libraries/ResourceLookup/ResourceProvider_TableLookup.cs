using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


internal sealed class ResourceProvider_TableLookup : IResourceProvider
{
	[SerializeField] DefaultResourceLookupTable _defaultResourceTable;
	private Dictionary<string, UnityEngine.Object> _dictionary;

	public void Setup()
	{
		_dictionary = _defaultResourceTable.Table.Resources.ToDictionary(kvp => kvp.Guid, kvp => kvp.Object);
		_defaultResourceTable = null; // Don't need this hogging memory any more
	}

	public T Load<T>(string guid) where T : UnityEngine.Object
	{
		throw new NotImplementedException();
	}

	public IEnumerable<T> LoadAll<T>() where T : UnityEngine.Object
	{
		throw new NotImplementedException();
	}

}

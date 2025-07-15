using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for doing Resource.Load operations
/// Ideally I shouldn't even be using the resource folder.
/// I should be using Unity's fancy new "Addressables" thing
/// But IDK, that's a little overkill for a low-poly game with 64x64 textures
/// </summary>
public static class ResourceLoader
{
	static Dictionary<Type, Dictionary<string, UnityEngine.Object>> _caches = new();

	public static T Load<T>(string id) where T : UnityEngine.Object, IHasUniqueAssetId
	{
		var cache = GetCache<T>();

		if (cache.TryGetValue(id, out UnityEngine.Object obj))
		{
			return (T)obj;
		}
		else
		{
			throw new ArgumentException($"Failed to load id {id} for {typeof(T)}");
		}
	}
	public static IEnumerable<T> LoadAll<T>() where T : UnityEngine.Object, IHasUniqueAssetId
	{
		var cache = GetCache<T>();

		return cache.Values.Select(v => (T)v);
	}

	/// <summary>
	/// Kind of a hack to support MixTexture loading, which is only done in one place and doesn't have unique asset IDs
	/// </summary>
	public static IEnumerable<T> LoadAllNoCache<T>() where T : UnityEngine.Object
	{
		var type = typeof(T);
		string folder = type.Name;
		var all = Resources.LoadAll<T>(folder);
		return all;
	}

	static Dictionary<string, UnityEngine.Object> GetCache<T>() where T : UnityEngine.Object, IHasUniqueAssetId
	{
		var type = typeof(T);
		if (!_caches.TryGetValue(type, out var cache))
		{
			// Load everything
			cache = new Dictionary<string, UnityEngine.Object>();
			string folder = type.Name;
			var all = Resources.LoadAll<T>(folder);
			foreach (var item in all)
			{
				cache[item.UniqueAssetID] = item;
			}
			_caches[type] = cache;
		}
		return cache;
	}

}

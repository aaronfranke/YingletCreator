using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Based off the Unity Engine's addressables AssetReferenceT
/// I ultimately found that system far too heavy handed to use
/// </summary>
[Serializable]
public class AssetReferenceT<T> where T : UnityEngine.Object
{
	// Using this naming convention for compatibility reasons after migrating off of Addressables
	[SerializeField] string m_AssetGUID;

	bool _cached = false; // Can't rely on nullability because the value might be null
	T _cachedVal;

	public T LoadSync()
	{
		if (!_cached)
		{
			_cachedVal = LoadValImpl();
			_cached = true;
		}

		return _cachedVal;
	}

	T LoadValImpl()
	{
		if (string.IsNullOrWhiteSpace(m_AssetGUID)) return null;

		// Some of this is used in the editor where addressables aren't available, so we need a backdoor
#if UNITY_EDITOR
		// if (!UnityEditor.EditorApplication.isPlaying)
		{
			var path = UnityEditor.AssetDatabase.GUIDToAssetPath(m_AssetGUID);
			var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
			return asset;
		}
#endif

		return null;
	}


	public static IEnumerable<T> LoadAllOfTypeSync()
	{
#if UNITY_EDITOR
		// if (!UnityEditor.EditorApplication.isPlaying)
		{
			string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");

			return guids.Select(guid =>
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

			}); // No ToArray because the consumers are all doing it
		}
#endif

		// TTODO
		//var label = typeof(TObject).Name;

		//var op = Addressables.LoadAssetsAsync<T>(label, null);
		//var results = op.WaitForCompletion();

		//return results;
		return Enumerable.Empty<T>();
	}

}
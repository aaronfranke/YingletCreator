using System;
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
			_cachedVal = CompositeResourceLoader.Instance.Load<T>(m_AssetGUID);
			_cached = true;
		}

		return _cachedVal;
	}
}
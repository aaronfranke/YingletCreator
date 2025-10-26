using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressableExtensionMethods
{
	public static T LoadSync<T>(this AssetReferenceT<T> assetReference) where T : Object
	{
		// Some of this is used in the editor where addressables aren't available, so we need a backdoor
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
		{
			var path = UnityEditor.AssetDatabase.GUIDToAssetPath(assetReference.AssetGUID);
			var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
			return asset;
		}
#endif

		var op = Addressables.LoadAssetAsync<T>(assetReference);
		T obj = op.WaitForCompletion();
		return obj;
	}

	public static IEnumerable<T> LoadAllOfTypeSync<T>() where T : Object
	{
		var label = typeof(T).Name;

		var op = Addressables.LoadAssetsAsync<T>(label, null);
		var results = op.WaitForCompletion();

		return results;
	}
}

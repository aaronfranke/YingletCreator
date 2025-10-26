using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressableExtensionMethods
{
	public static T LoadSync<T>(this AssetReferenceT<T> assetReference) where T : Object
	{

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
}

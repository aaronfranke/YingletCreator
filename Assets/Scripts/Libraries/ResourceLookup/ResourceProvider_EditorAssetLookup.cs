using System.Collections.Generic;
using System.Linq;

internal interface IResourceProvider
{
	T Load<T>(string guid) where T : UnityEngine.Object;
	IEnumerable<T> LoadAll<T>() where T : UnityEngine.Object;
}

internal sealed class ResourceProvider_EditorAssetLookup : IResourceProvider
{
	public T Load<T>(string guid) where T : UnityEngine.Object
	{
		if (string.IsNullOrWhiteSpace(guid)) return null;

#if UNITY_EDITOR
		var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
		var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
		return asset;
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return null;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public IEnumerable<T> LoadAll<T>() where T : UnityEngine.Object
	{
#if UNITY_EDITOR
		string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");

		return guids.Select(guid =>
		{
			var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

		}); // No ToArray because the consumers are all doing it
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return Enumerable.Empty<T>();
#pragma warning restore CS0162 // Unreachable code detected
	}
}

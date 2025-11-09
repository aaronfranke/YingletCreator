using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ObjectExtensionMethods
{
	public static string GetAssetPath(this Object obj)
	{
		return AssetDatabase.GetAssetPath(obj);
	}

	public static string GetParentFolder(this Object obj)
	{
		return Path.GetDirectoryName(obj.GetAssetPath());
	}

	public static IEnumerable<T> LoadAllAssets<T>(string rootFolder = null) where T : Object
	{
		string[] searchFolders = string.IsNullOrWhiteSpace(rootFolder) ? null : new[] { rootFolder };
		var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", searchFolders);
		var modDefinitions = guids
			.Select(guid =>
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<T>(path);
				return asset;
			});
		return modDefinitions;
	}
}

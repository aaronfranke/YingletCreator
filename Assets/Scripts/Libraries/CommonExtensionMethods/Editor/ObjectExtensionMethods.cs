using System.IO;
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
}

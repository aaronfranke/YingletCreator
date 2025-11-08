using UnityEditor;
using UnityEngine;

// Adapted from https://discussions.unity.com/t/scriptable-object-instance-ids-how-to-save-a-proper-reference/921345/22
public class UniqueAssetIdPostProcessor : AssetPostprocessor
{
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		SerializeUniqueAssetIds(importedAssets);
	}

	static void SerializeUniqueAssetIds(string[] assetPaths)
	{
		bool save = false;

		foreach (var assetPath in assetPaths)
		{
			// Early optimization to avoid loading the object for anything but scriptable objects
			if (!assetPath.EndsWith(".asset")) continue;

			Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
			if (assetObject is not IHasUniqueAssetId iHasUniqueAssetID)
			{
				continue;
			}
			string assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
			if (assetGuid == iHasUniqueAssetID.UniqueAssetID)
			{
				continue;
			}
			iHasUniqueAssetID.UniqueAssetID = assetGuid;
			EditorUtility.SetDirty(assetObject);

			save = true;
			Debug.Log("Updated unique asset id on serialized object: " + assetObject.name);
		}
		if (save)
		{
			AssetDatabase.SaveAssets();
		}
	}
}

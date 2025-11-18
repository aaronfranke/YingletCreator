using Character.Compositor;
using Character.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor logic that I'm only really going to run like, once
/// </summary>
public class SandboxLogic : MonoBehaviour
{

	[MenuItem("Custom/Run sandbox logic script")]
	static void RunSandboxLogicScript()
	{
		AssignFbxSubAssetsToMeshWithMaterial();
	}

	static void RenameAssets()
	{
		string[] guids = AssetDatabase.FindAssets("t:MeshWithMaterial");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<MeshWithMaterial>(path);
			AssetDatabase.RenameAsset(path, CleanString(asset.name));

		}
		AssetDatabase.SaveAssets();


		static string CleanString(string input)
		{
			// No spaces or underlines
			return input.Replace(" ", "")
						.Replace("_", "-");
		}
	}

	static void AssignFbxSubAssetsToMeshWithMaterial()
	{
		string[] guids = AssetDatabase.FindAssets("t:MeshWithMaterial");
		Dictionary<string, MeshWithMaterial> nameToScriptableDict = new();
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<MeshWithMaterial>(path);
			nameToScriptableDict[asset.name] = asset;
		}

		var assetPath = "Assets/Art/Models/Entities/Yinglet/Yinglet-Clothes-Outfits.fbx";
		var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
		foreach (var asset in assets)
		{
			if (asset is GameObject gameObject)
			{
				if (gameObject.GetComponent<SkinnedMeshRenderer>() == null) continue;
				if (nameToScriptableDict.TryGetValue(asset.name, out var scriptable))
				{
					scriptable.EditorSetSkinnedMeshRendererPrefab(gameObject);
					EditorUtility.SetDirty(scriptable);
				}
				else
				{
					Debug.LogWarning($"No ScriptableObject found for {asset.name}");
				}
			}
		}
		AssetDatabase.SaveAssets();
	}

	static void UpdateAssetDatabase()
	{

		string[] guids = AssetDatabase.FindAssets("t:CharacterToggleId");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<CharacterToggleId>(path);

			//var referencePath = AssetDatabase.GetAssetPath(asset.Preview._cameraPosition);
			//if (string.IsNullOrWhiteSpace(referencePath))
			//{
			//	Debug.LogWarning($"{path} did not have a reference to set");
			//	continue;
			//}
			//var referenceGuid = AssetDatabase.AssetPathToGUID(referencePath);
			//if (string.IsNullOrEmpty(referenceGuid))
			//{
			//	Debug.LogWarning($"Added {referencePath} not found in AssetDatabase");
			//	continue;
			//}
			//GetOrCreateEntry(referenceGuid);
			//asset.Preview._cameraPositionReference = new(referenceGuid);

			//var list = new List<AssetReferenceT<CharacterToggleEnforcementGroup>>();
			//var items = asset._groups;
			//if (items == null) continue;
			//foreach (var added in items)
			//{
			//	var addedPath = AssetDatabase.GetAssetPath(added);
			//	var addedGuid = AssetDatabase.AssetPathToGUID(addedPath);
			//	if (string.IsNullOrEmpty(guid))
			//	{
			//		Debug.LogWarning($"Added {added.name} not found in AssetDatabase");
			//		continue;
			//	}
			//	GetOrCreateEntry(addedGuid);
			//	list.Add(new(addedGuid));
			//}
			//asset._groupReferences = list.ToArray();

			EditorUtility.SetDirty(asset);
		}
		AssetDatabase.SaveAssets();
	}
}

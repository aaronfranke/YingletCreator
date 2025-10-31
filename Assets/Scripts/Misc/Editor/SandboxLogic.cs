using Character.Data;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

/// <summary>
/// Editor logic that I'm only really going to run like, once
/// </summary>
public class SandboxLogic : MonoBehaviour
{

	[MenuItem("Custom/Run sandbox logic script")]
	static void RunSandboxLogicScript()
	{
		AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

		string[] guids = AssetDatabase.FindAssets("t:PoseId");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<PoseId>(path);

			//var referencePath = AssetDatabase.GetAssetPath(asset._toReplace);
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
			//asset._toReplaceReference = new(referenceGuid);

			//var list = new List<AssetReferenceT<MeshWithMaterial>>();
			//var items = asset._props;
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
			//asset._propReferences = list.ToArray();

			EditorUtility.SetDirty(asset);
		}
		AssetDatabase.SaveAssets();


#pragma warning disable CS8321 // Local function is declared but never used
		void GetOrCreateEntry(string referenceGuid)
		{

			AddressableAssetEntry entry = settings.FindAssetEntry(referenceGuid);
			if (entry == null)
			{
				// Add new entry
				entry = settings.CreateOrMoveEntry(referenceGuid, settings.DefaultGroup);
				entry.address = referenceGuid;
			}
		}
#pragma warning restore CS8321 // Local function is declared but never used
	}

}

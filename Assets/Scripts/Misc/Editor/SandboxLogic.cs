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
		//string[] guids = AssetDatabase.FindAssets("t:CharacterToggleId");
		//foreach (var guid in guids)
		//{
		//	string path = AssetDatabase.GUIDToAssetPath(guid);
		//	CharacterToggleId asset = AssetDatabase.LoadAssetAtPath<CharacterToggleId>(path);

		//	var list = new List<AssetReferenceT<MixTexture>>();

		//	foreach (var added in asset._addedTextures)
		//	{
		//		var addedPath = AssetDatabase.GetAssetPath(added);
		//		var addedGuid = AssetDatabase.AssetPathToGUID(addedPath);
		//		if (string.IsNullOrEmpty(guid))
		//		{
		//			Debug.LogWarning($"Added {added.name} not found in AssetDatabase");
		//			continue;
		//		}
		//		list.Add(new AssetReferenceT<MixTexture>(addedGuid));
		//	}
		//	asset._addedTextureReferences = list.ToArray();

		//	EditorUtility.SetDirty(asset);
		//	AssetDatabase.SaveAssets();
		//}
	}
}

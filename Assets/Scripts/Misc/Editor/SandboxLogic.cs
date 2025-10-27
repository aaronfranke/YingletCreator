using Character.Compositor;
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
		string[] guids = AssetDatabase.FindAssets("t:MixTexture");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			MixTexture asset = AssetDatabase.LoadAssetAtPath<MixTexture>(path);


			//var addedPath = AssetDatabase.GetAssetPath(asset.Order._group);
			//var addedGuid = AssetDatabase.AssetPathToGUID(addedPath);
			//if (string.IsNullOrEmpty(guid))
			//{
			//	Debug.LogWarning($"Added {asset.Order._group.name} not found in AssetDatabase");
			//	continue;
			//}
			//asset.Order._groupReference = new(addedGuid);







			//var list = new List<AssetReferenceT<MixTexture>>();
			//foreach (var added in asset._addedTextures)
			//{
			//	var addedPath = AssetDatabase.GetAssetPath(added);
			//	var addedGuid = AssetDatabase.AssetPathToGUID(addedPath);
			//	if (string.IsNullOrEmpty(guid))
			//	{
			//		Debug.LogWarning($"Added {added.name} not found in AssetDatabase");
			//		continue;
			//	}
			//	list.Add(new AssetReferenceT<MixTexture>(addedGuid));
			//}
			//asset._addedTextureReferences = list.ToArray();

			EditorUtility.SetDirty(asset);
		}
		AssetDatabase.SaveAssets();
	}
}

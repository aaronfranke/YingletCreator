using System.IO;
using CharacterCompositor;
using UnityEditor;
using UnityEngine;

public class UpdateEyeAssets
{
	const string RAW_TEXTURE_PATH = "Assets/Art/Materials/Entities/Yinglet/Eyes/RawTextures";
	const string SCRIPTABLE_OBJECT_OUTPUT_PATH = "Assets/ScriptableObjects/CharacterCompositor/MixTexture/Eyes/";

	[MenuItem("Custom/Update Eye Assets")]
	static void Apply()
	{
		var eyeTextureFolders = Directory.GetDirectories(RAW_TEXTURE_PATH);
		foreach (var eyeTextureFolder in eyeTextureFolders)
		{
			var destinationName = Path.GetFileName(eyeTextureFolder);
			var destinationPath = Path.Combine(SCRIPTABLE_OBJECT_OUTPUT_PATH, $"{destinationName}.asset");
			var asset = AssetDatabase.LoadAssetAtPath<EyeMixTextures>(destinationPath);

			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<EyeMixTextures>();
				AssetDatabase.CreateAsset(asset, destinationPath);
			}

			asset._pupil = LoadTex("Pupil");

			EditorUtility.SetDirty(asset);

			Texture2D LoadTex(string relativePath)
			{
				var expectedPath = Path.Combine(eyeTextureFolder, relativePath + ".png");
				var tex2d = AssetDatabase.LoadAssetAtPath<Texture2D>(expectedPath);
				if (tex2d == null)
				{
					Debug.LogWarning($"Failed to find expected tex2d at {expectedPath}");
				}
				return tex2d;
			}
		}
		AssetDatabase.SaveAssets();
	}
}

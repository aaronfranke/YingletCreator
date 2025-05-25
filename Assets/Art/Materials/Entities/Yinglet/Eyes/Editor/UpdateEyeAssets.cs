using Character.Compositor;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This script used to do a lot more work, when each expression was separate
/// It would stitch them all together
/// Now it just generates the scriptable objects for the eyes
/// Which could honestly be done manually but whatever I'll take a little automation
/// </summary>
public class UpdateEyeAssets
{
	const string RAW_TEXTURE_PATH = "Assets/Art/Materials/Entities/Yinglet/Eyes/RawTextures";
	const string SCRIPTABLE_OBJECT_OUTPUT_PATH = "Assets/ScriptableObjects/CharacterCompositor/MixTexture/Eyes/";

	[MenuItem("Custom/Update Eye Assets")]
	static void Apply()
	{
		// Might eventually allow this to be overwritten if an eye wants a specific pupil instead
		var pupil = LoadTex(Path.Combine(RAW_TEXTURE_PATH, "Pupil.png"));

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

			var fill = LoadTex(Path.Combine(eyeTextureFolder, "Fill.png"));
			var eyelid = LoadTex(Path.Combine(eyeTextureFolder, "Eyelid.png"));
			asset.EditorSetTextures(fill, eyelid, pupil);

			EditorUtility.SetDirty(asset);
			AssetDatabase.SaveAssets();
		}
	}


	static Texture2D LoadTex(string path)
	{
		var expectedPath = Path.Combine(path);
		var tex2d = AssetDatabase.LoadAssetAtPath<Texture2D>(expectedPath);
		if (tex2d == null)
		{
			Debug.LogWarning($"Failed to find expected tex2d at {expectedPath}");
		}
		tex2d.alphaIsTransparency = true;
		EditorUtility.SetDirty(tex2d);
		return tex2d;
	}
}

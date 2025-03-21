using System.IO;
using System.Linq;
using CharacterCompositor;
using UnityEditor;
using UnityEngine;

public class UpdateEyeAssets
{
	const string RAW_TEXTURE_PATH = "Assets/Art/Materials/Entities/Yinglet/Eyes/RawTextures";
	const string SCRIPTABLE_OBJECT_OUTPUT_PATH = "Assets/ScriptableObjects/CharacterCompositor/MixTexture/Eyes/";

	static readonly string[] EYE_EXPRESSIONS = new[] { "Normal", "Normal" , "Normal" };

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
			var textureSize = EYE_EXPRESSIONS.Length;

			asset._pupil = LoadTex("Pupil");
			asset._outline = GenerateAndLoadTex("Outline");
			asset._fill = GenerateAndLoadTex("Fill");

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

			Texture2D GenerateAndLoadTex(string textureName)
			{
				var texture2Ds = EYE_EXPRESSIONS.Select(e => LoadTex(Path.Combine(e, textureName))).ToArray();
				int texSize = texture2Ds.FirstOrDefault(t => t != null)?.width ?? 64;

				Texture2D combinedTexture = new Texture2D(texSize * texture2Ds.Length, texSize);
				for (int i = 0; i < texture2Ds.Length; i++)
				{
					var texture2D = texture2Ds[i];
					if (texture2D == null) continue;
					combinedTexture.SetPixels(i * texSize, 0, texSize, texSize, texture2D.GetPixels());
				}

				byte[] pngData = combinedTexture.EncodeToPNG();
				var outputName = $"Generated_{textureName}";
				var outputPath = Path.Combine(eyeTextureFolder, $"{outputName}.png");
				File.WriteAllBytes(outputPath, pngData);
				AssetDatabase.Refresh();

				var generatedTex = LoadTex(outputName);
				return generatedTex;
			}
		}
		AssetDatabase.SaveAssets();
	}
}

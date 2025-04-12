using CharacterCompositor;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UpdateEyeAssets
{
	const int SIZE = 64;
	const string RAW_TEXTURE_PATH = "Assets/Art/Materials/Entities/Yinglet/Eyes/RawTextures";
	const string SCRIPTABLE_OBJECT_OUTPUT_PATH = "Assets/ScriptableObjects/CharacterCompositor/MixTexture/Eyes/";

	[MenuItem("Custom/Update Eye Assets")]
	static void Apply()
	{
		var transparentPixels = CreateTransparentPixels();
		var expressionNames = Enum.GetNames(typeof(EyeExpression));

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
			var textureSize = expressionNames.Length;

			asset._pupil = LoadTex("Pupil");
			asset._outline = GenerateAndLoadTex("Outline", true);
			asset._fill = GenerateAndLoadTex("Fill", false);
			if (ContainsEyelidFile(eyeTextureFolder))
			{
				asset._eyelid = GenerateAndLoadTex("Eyelid", false);
			}
			else
			{
				asset._eyelid = null;
			}

			EditorUtility.SetDirty(asset);
			AssetDatabase.SaveAssets();

			Texture2D LoadTex(string relativePath, bool mustExist = true)
			{
				var expectedPath = Path.Combine(eyeTextureFolder, relativePath + ".png");
				var tex2d = AssetDatabase.LoadAssetAtPath<Texture2D>(expectedPath);
				if (tex2d == null && mustExist)
				{
					Debug.LogWarning($"Failed to find expected tex2d at {expectedPath}");
				}
				return tex2d;
			}

			Texture2D GenerateAndLoadTex(string textureName, bool mustExist)
			{
				var expressionRelativePaths = expressionNames.Select(e => Path.Combine(e, textureName)).ToArray();
				// Ensure all the textures are readable
				foreach (var relPath in expressionRelativePaths)
				{
					UpdateTextureSettings(Path.Combine(eyeTextureFolder, relPath + ".png"), false);
				}
				var texture2Ds = expressionRelativePaths.Select(e => LoadTex(e, mustExist)).ToArray();
				// int texSize = texture2Ds.FirstOrDefault(t => t != null)?.width ?? 64;
				int texSize = SIZE;

				Texture2D combinedTexture = new Texture2D(texSize * texture2Ds.Length, texSize);
				for (int i = 0; i < texture2Ds.Length; i++)
				{
					var texture2D = texture2Ds[i];
					var pixelsToApply = texture2D?.GetPixels() ?? transparentPixels;
					combinedTexture.SetPixels(i * texSize, 0, texSize, texSize, pixelsToApply);
				}

				byte[] pngData = combinedTexture.EncodeToPNG();
				var outputName = $"Generated_{textureName}";
				var outputPath = Path.Combine(eyeTextureFolder, $"{outputName}.png");
				File.WriteAllBytes(outputPath, pngData);
				AssetDatabase.Refresh();
				UpdateTextureSettings(outputPath, false);

				var generatedTex = LoadTex(outputName);
				return generatedTex;
			}

			void UpdateTextureSettings(string path, bool compress)
			{
				var importer = AssetImporter.GetAtPath(path) as TextureImporter;
				if (importer == null) return; // This asset may not exist
				importer.isReadable = true;
				importer.alphaIsTransparency = true;
				importer.compressionQuality = compress ? 3 : 100; // Source textures shouldn't be compressed
				importer.sRGBTexture = true; // These are used for grayscale
				importer.SaveAndReimport();
			}
		}
	}

	static Color[] CreateTransparentPixels()
	{
		Texture2D texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);

		Color transparentColor = new Color(0, 0, 0, 0); // Fully transparent
		Color[] pixels = new Color[SIZE * SIZE];

		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = transparentColor;
		}

		return pixels;
	}

	static bool ContainsEyelidFile(string folderPath)
	{
		// Search for the file in the given directory and all subdirectories
		string[] files = Directory.GetFiles(folderPath, "Eyelid.png", SearchOption.AllDirectories);
		return files.Length > 0;
	}
}

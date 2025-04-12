using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class TextureGrayscaler : EditorWindow
{
	[MenuItem("Custom/Texture Processing/Grayscale and Normalize")]
	public static void ShowWindow()
	{
		GetWindow<TextureGrayscaler>(nameof(TextureGrayscaler));
	}

	private Color lightColor = Color.white;
	private Color darkColor = Color.black;

	private void OnGUI()
	{
		GUILayout.Label("Select Two Colors", EditorStyles.boldLabel);

		lightColor = EditorGUILayout.ColorField("LightColor", lightColor);
		darkColor = EditorGUILayout.ColorField("DarkColor", darkColor);

		if (GUILayout.Button("Grayscale"))
		{
			GrayscaleAndNormalizeTexture(lightColor, darkColor);
		}
	}

	public static void GrayscaleAndNormalizeTexture(Color lightColor, Color darkColor)
	{
		Texture2D selectedTexture = Selection.activeObject as Texture2D;

		if (selectedTexture == null)
		{
			Debug.LogError("Please select a texture.");
			return;
		}

		string assetPath = AssetDatabase.GetAssetPath(selectedTexture);
		TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
		var originalTextureCompression = importer.textureCompression;

		UpdateTextureSettings();


		// Get pixel colors
		Texture2D tex = new Texture2D(selectedTexture.width, selectedTexture.height, TextureFormat.RGBA32, false);
		tex.SetPixels(selectedTexture.GetPixels());
		tex.Apply();

		Color[] pixels = tex.GetPixels();


		// Normalize and apply back to pixels
		for (int i = 0; i < pixels.Length; i++)
		{
			var col = pixels[i];
			float normalized = InverseLerpColor(darkColor, lightColor, col);
			// Capture it to only 50% of the range
			// We'll use the rest of that range for black/white details
			normalized = normalized / 2 + .25f;
			pixels[i] = new Color(normalized, normalized, normalized, col.a);
		}

		tex.SetPixels(pixels);
		tex.Apply();

		// Save the new texture
		byte[] pngData = tex.EncodeToPNG();
		if (pngData != null)
		{
			string path = Path.GetDirectoryName(assetPath);
			string newPath = Path.Combine(path, selectedTexture.name + ".png");
			File.WriteAllBytes(newPath, pngData);
			AssetDatabase.Refresh();
			Debug.Log("Texture processed and saved at: " + newPath);
		}
		FinalizeSettings();


		void UpdateTextureSettings()
		{
			bool needsUpdate = false;
			if (!importer.isReadable)
			{
				importer.isReadable = true;
				needsUpdate = true;
			}
			if (importer.textureCompression != TextureImporterCompression.Uncompressed)
			{
				importer.textureCompression = TextureImporterCompression.Uncompressed;
				needsUpdate = true;
			}

			if (needsUpdate)
			{
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
			}
		}


		void FinalizeSettings()
		{
			importer.textureCompression = originalTextureCompression;
			AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
		}
	}

	// chat-gpt generated garbage
	static float InverseLerpColor(Color colorA, Color colorB, Color inputColor)
	{
		float diffR = Mathf.Abs(colorB.r - colorA.r);
		float diffG = Mathf.Abs(colorB.g - colorA.g);
		float diffB = Mathf.Abs(colorB.b - colorA.b);

		if (diffR >= diffG && diffR >= diffB)
			return Mathf.InverseLerp(colorA.r, colorB.r, inputColor.r);
		else if (diffG >= diffR && diffG >= diffB)
			return Mathf.InverseLerp(colorA.g, colorB.g, inputColor.g);
		else
			return Mathf.InverseLerp(colorA.b, colorB.b, inputColor.b);
	}
}

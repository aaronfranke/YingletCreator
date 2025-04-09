using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class TextureGrayscaler
{
	[MenuItem("Custom/Texture Processing/Grayscale and Normalize")]
	public static void GrayscaleAndNormalizeTexture()
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
		float minGray = 1f;
		float maxGray = 0f;

		// Peach colored overrides
		//minGray = 0.7104314f;
		//maxGray = 0.7905334f;

		ColorRA[] grays = new ColorRA[pixels.Length];

		// Convert to grayscale and find min/max
		for (int i = 0; i < pixels.Length; i++)
		{
			Color color = pixels[i];
			float gray = color.grayscale;
			grays[i].R = gray;
			grays[i].A = color.a;

			if (color.a < .3f) continue; // Don't trust transparent colors; The program may have defaulted their RGB to black
			if (gray < minGray) minGray = gray;
			if (gray > maxGray) maxGray = gray;
		}
		Debug.Log($"Min gray: {minGray}");
		Debug.Log($"Max gray: {maxGray}");

		float range = maxGray - minGray;
		if (range <= 0f) range = 1f; // Avoid divide by zero

		// Normalize and apply back to pixels
		for (int i = 0; i < pixels.Length; i++)
		{
			var gray = grays[i];
			float normalized = (gray.R - minGray) / range;
			// Capture it to only 50% of the range
			// We'll use the rest of that range for black/white details
			normalized = normalized / 2 + .25f;
			pixels[i] = new Color(normalized, normalized, normalized, gray.A);
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
		RevertCompression();


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
			if (importer.sRGBTexture == true)
			{
				importer.sRGBTexture = false;
				needsUpdate = true;
			}

			if (needsUpdate)
			{
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
			}
		}


		void RevertCompression()
		{

			if (importer.textureCompression != originalTextureCompression)
			{
				importer.textureCompression = originalTextureCompression;
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
			}
		}
	}


	struct ColorRA
	{
		public ColorRA(float r, float a)
		{
			R = r;
			A = a;
		}
		public float R;
		public float A;
	}
}

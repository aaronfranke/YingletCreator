using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MouthComposite))]
public class MouthCompositeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();

		if (GUILayout.Button("Composite"))
		{
			MouthComposite mouthComposite = (MouthComposite)target;

			var inputsTex2Ds = new Texture2D[] { mouthComposite.Grin, mouthComposite.Frown, mouthComposite.Muse };
			var inputPaths = inputsTex2Ds.Select(t => AssetDatabase.GetAssetPath(t)).ToArray();

			string assetPath = AssetDatabase.GetAssetPath(mouthComposite);
			string assetDir = Path.GetDirectoryName(assetPath);
			string assetName = Path.GetFileNameWithoutExtension(assetPath);
			string outputPath = Path.Combine(assetDir, assetName + "-Generated.png");

			GenerateCompositeTexture(inputPaths, outputPath);

			if (!mouthComposite.HasMask) return;

			var inputMaskPaths = inputPaths.Select(path => path.Replace(".png", "-Mask.png")).ToArray();
			string outputMaskPath = Path.Combine(assetDir, assetName + "-GeneratedMask.png");
			GenerateCompositeTexture(inputMaskPaths, outputMaskPath);
		}

		static void GenerateCompositeTexture(IEnumerable<string> paths, string outputPath)
		{
			// Update settings on all input textures
			foreach (var path in paths)
			{
				TexGenerationUtils.UpdateTextureSettings(path, false);
			}

			var textures = paths.Select(path => AssetDatabase.LoadAssetAtPath<Texture2D>(path)).ToArray();
			var firstTexture = textures[0];

			// Assume all input textures are the same size
			Vector2Int individualSize = new Vector2Int(firstTexture.width, firstTexture.height);

			// Create a composite texture
			Vector2Int compositeSize = new Vector2Int(individualSize.x * (int)MouthExpression.MAX, individualSize.y);
			Texture2D composite = new Texture2D(compositeSize.x, compositeSize.y, TextureFormat.RGBA32, false);

			// Make transparent
			var transparentPixels = TexGenerationUtils.CreateTransparentPixels(compositeSize);
			composite.SetPixels(transparentPixels);

			// Fill the composite texture: 
			for (int i = 0; i < textures.Length; i++)
			{
				Texture2D texture = textures[i];

				Color[] pixels = texture.GetPixels();
				composite.SetPixels(i * individualSize.x, 0, individualSize.x, individualSize.y, pixels);
			}
			composite.Apply();

			// Encode to PNG and write to disk
			byte[] pngData = composite.EncodeToPNG();
			File.WriteAllBytes(outputPath, pngData);

			// Refresh the asset database so the new file appears in the project
			AssetDatabase.Refresh();
			TexGenerationUtils.UpdateTextureSettings(outputPath, true);
		}
	}
}

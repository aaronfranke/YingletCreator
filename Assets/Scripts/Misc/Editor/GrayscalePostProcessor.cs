using CharacterCompositor;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public sealed class GrayscalePostProcessor : AssetPostprocessor
{

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		var sw = Stopwatch.StartNew();
		foreach (var assetPath in importedAssets)
		{
			if (!assetPath.EndsWith(".asset")) continue;

			var mixTexture = AssetDatabase.LoadAssetAtPath<MixTexture>(assetPath);
			if (mixTexture == null) continue;
			UpdateGrayscale(mixTexture.Grayscale);
		}
		//UnityEngine.Debug.Log($"GrayscalePostProcessor finished in {sw.ElapsedMilliseconds}ms");
	}

	static void UpdateGrayscale(Texture2D tex)
	{
		if (tex == null) return;
		if (tex.isDataSRGB) ; // probably not setup yet
		{
			var pathToShading = AssetDatabase.GetAssetPath(tex);
			TextureImporter importer = AssetImporter.GetAtPath(pathToShading) as TextureImporter;
			importer.sRGBTexture = false;
			AssetDatabase.ImportAsset(pathToShading, ImportAssetOptions.ForceUpdate);
			UnityEngine.Debug.Log($"Updated texture to linear: {pathToShading}");
		}
	}
}
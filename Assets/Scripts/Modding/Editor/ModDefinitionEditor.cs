using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModDefinition))]
public class ModDefinitionEditor : Editor
{
	const string ModExtension = ".yingmod";

	SerializedProperty _modDisplayTitleProp;

	void OnEnable()
	{
		_modDisplayTitleProp = serializedObject.FindProperty("_modDisplayTitle");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("This is the title that will display in-game and on the Steam workshop:");
		EditorGUILayout.PropertyField(_modDisplayTitleProp);

		DrawHorizontalLine(Color.gray);


		EditorGUILayout.LabelField("Click the following button to generate a mod (.bundle) file. This bundle will include:");
		EditorGUILayout.LabelField(" • Presets (.yingsave)");
		EditorGUILayout.LabelField(" • Toggles (CharacterToggleId)");
		EditorGUILayout.LabelField(" • Poses (PoseID)");
		EditorGUILayout.LabelField("Supporting textures, models, and ScriptableObject metadata will also be bundled.");

		var modDefinition = (ModDefinition)target;

		if (GUILayout.Button("Bundle Content"))
		{
			BundleContent(modDefinition);
		}

		serializedObject.ApplyModifiedProperties();
	}

	private static void DrawHorizontalLine(Color color, float thickness = 1f, float padding = 6f)
	{
		EditorGUILayout.Space();

		Rect rect = EditorGUILayout.GetControlRect(false, thickness + padding);
		rect.height = thickness;
		rect.y += padding / 2f;
		EditorGUI.DrawRect(rect, color);
		EditorGUILayout.Space();
	}

	static void BundleContent(ModDefinition modDefinition)
	{
		try
		{
			BundleContentImpl(modDefinition);
		}
		catch (System.Exception ex)
		{
			EditorUtility.ClearProgressBar();
			Debug.LogException(ex);
			EditorUtility.DisplayDialog("Bundle Content", $"An error occurred while bundling: {ex.Message}\nSee Console for details.", "OK");
		}
	}

	static void BundleContentImpl(ModDefinition modDefinition)
	{
		string modAssetPath = AssetDatabase.GetAssetPath(modDefinition);
		var relativeFolder = Path.GetDirectoryName(modAssetPath);
		string bundleFileName = Path.GetFileNameWithoutExtension(modAssetPath) + ModExtension;

		var assetPaths = GetAssetPathsInFolder(relativeFolder);

		AssignAssetBundleToAssets(assetPaths, bundleFileName);

		EditorUtility.DisplayProgressBar("Building Bundle", $"Building {bundleFileName}...", 0.5f);

		var build = new AssetBundleBuild()
		{
			assetBundleName = bundleFileName,
			assetNames = assetPaths
		};
		var manifest = BuildPipeline.BuildAssetBundles(relativeFolder, new[] { build }, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		DeleteAllManifestFiles(relativeFolder);

		EditorUtility.ClearProgressBar();

		if (manifest == null)
		{
			EditorUtility.DisplayDialog("Bundle Content", "BuildAssetBundles returned null. See Console for details.", "OK");
			return;
		}

		AssetDatabase.Refresh();

		// Inform user and show the resulting file path
		string outputBundlePath = Path.Combine(relativeFolder, bundleFileName);
		EditorUtility.DisplayDialog("Bundle Content", $"Bundle built: {outputBundlePath}", "OK");
	}

	static string[] GetAssetPathsInFolder(string relativeFolder)
	{
		string[] IgnoreExtensions = new[] { ".manifest", "", ModExtension }; // Don't consume anything generated or any folders

		var assetGuids = AssetDatabase.FindAssets("", new[] { relativeFolder });
		return assetGuids
			.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
			.Where(path =>
			{
				var ext = Path.GetExtension(path) ?? string.Empty;
				return !IgnoreExtensions.Any(ignore => string.Equals(ext, ignore, StringComparison.OrdinalIgnoreCase));
			})
			.ToArray();
	}

	static void AssignAssetBundleToAssets(string[] assetPaths, string bundleName)
	{
		foreach (var assetPath in assetPaths)
		{
			var importer = AssetImporter.GetAtPath(assetPath);
			if (importer != null && importer.assetBundleName != bundleName)
			{
				importer.assetBundleName = bundleName;
				importer.SaveAndReimport();
			}
		}
		AssetDatabase.RemoveUnusedAssetBundleNames(); // clean up any stale bundle names
	}

	static void DeleteAllManifestFiles(string relativeFolder)
	{
		string fullPath = Path.GetFullPath(relativeFolder);

		// Delete all .manifest files (recursive)
		foreach (string file in Directory.GetFiles(fullPath, "*.manifest", SearchOption.TopDirectoryOnly))
		{
			File.Delete(file);
		}

		// Delete the root manifest file (same name as folder, no extension)
		string rootManifestPath = Path.Combine(fullPath, Path.GetFileName(fullPath));
		if (File.Exists(rootManifestPath))
		{
			File.Delete(rootManifestPath);
		}
	}

}
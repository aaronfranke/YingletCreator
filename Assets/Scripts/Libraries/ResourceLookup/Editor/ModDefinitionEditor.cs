using Snapshotter;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModDefinition))]
public class ModDefinitionEditor : Editor
{
	SerializedProperty _modDisplayTitleProp;

	void OnEnable()
	{
		_modDisplayTitleProp = serializedObject.FindProperty("_modDisplayTitle");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var modDefinition = (ModDefinition)target;

		EditorGUILayout.LabelField("This is the title that will display on the about page:");
		EditorGUILayout.PropertyField(_modDisplayTitleProp);

		DrawHorizontalLine(Color.gray);
		EditorGUILayout.LabelField("Click the following button to generate toggle and pose icons");
		EditorGUILayout.LabelField("Note: This can only be done while the game is running in-editor");

		if (GUILayout.Button("Generate Toggle + Pose Icons"))
		{
			if (EditorApplication.isPlaying)
			{
				SnapshotToSpriteSheetUtils.GenerateToggleIcons(modDefinition);
				SnapshotToSpriteSheetUtils.GeneratePoseIcons(modDefinition);
				SnapshotToSpriteSheetUtils.UpdateIconsInScene();
				EditorUtility.DisplayDialog("Generate Icons", $"Icons generated!", "OK");
			}
			else
			{
				EditorUtility.DisplayDialog("Generate Icons", $"The game must be running to generate icons; hit the play button above.", "OK");
			}
		}

		DrawHorizontalLine(Color.gray);

		if (modDefinition.IsBuiltInMod)
		{
			EditorGUILayout.LabelField("The default mod can not be built; it is automatically included");

			if (GUILayout.Button("Regenerate Lookup Table"))
			{
				ResourceTablePopulationUtils.PopulateLookupTable(modDefinition);
			}
		}

		else
		{
			EditorGUILayout.LabelField("Click the following button to generate a mod (.bundle) file. This bundle will include all of the following items nested in this folder:");
			EditorGUILayout.LabelField(" • Presets (.yingsave)");
			EditorGUILayout.LabelField(" • Toggles (CharacterToggleId)");
			EditorGUILayout.LabelField(" • Poses (PoseID)");
			EditorGUILayout.LabelField("Supporting textures, models, and ScriptableObject metadata will also be bundled.");


			if (GUILayout.Button("Build Mod"))
			{
				BundleContent(modDefinition);
			}
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
			string modAssetPath = AssetDatabase.GetAssetPath(modDefinition);
			string modRelativeFolder = Path.GetDirectoryName(modAssetPath);
			string bundleFileName = Path.GetFileNameWithoutExtension(modAssetPath) + ModDefinition.ModExtension;
			string outputFolder = GetOutputFolder();

			EditorUtility.DisplayProgressBar($"Building Mod - {bundleFileName}", "Assigning assets to bundle...", 0.1f);
			var assetPaths = GetAssetPathsInFolder(modRelativeFolder);
			AssignAssetBundleToAssets(assetPaths, modDefinition.UniqueAssetID);

			EditorUtility.DisplayProgressBar($"Building Mod - {bundleFileName}", $"Creating asset lookup table...", 0.3f);
			ResourceTablePopulationUtils.PopulateLookupTable(modDefinition);

			EditorUtility.DisplayProgressBar($"Building Mod - {bundleFileName}", "Saving updated properties on ModDefinition...", 0.7f);
			EditorUtility.SetDirty(modDefinition);
			AssetDatabase.SaveAssets();

			EditorUtility.DisplayProgressBar($"Building Mod - {bundleFileName}", "Building bundle...", 0.7f);
			BuildBundle(outputFolder, bundleFileName, assetPaths);

			EditorUtility.DisplayProgressBar($"Building Mod - {bundleFileName}", "Cleaning up...", 0.9f);
			DeleteAllManifestFiles(outputFolder);

			EditorUtility.ClearProgressBar();
			EditorUtility.RevealInFinder(Path.Combine(outputFolder, bundleFileName));
			EditorUtility.DisplayDialog($"Mod Built - {bundleFileName}", $"Mod contents built to: {outputFolder}", "OK");
		}
		catch (Exception ex)
		{
			EditorUtility.ClearProgressBar();
			Debug.LogException(ex);
			EditorUtility.DisplayDialog("Bundle Content", $"An error occurred while bundling: {ex.Message}\nSee Console for details.", "OK");
		}
	}

	static string[] GetAssetPathsInFolder(string relativeFolder)
	{

		var assetGuids = AssetDatabase.FindAssets("", new[] { relativeFolder });
		return assetGuids
			.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
			.Where(path => !path.EndsWith(".yingsave"))
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

	static string GetOutputFolder()
	{
		var projectRoot = Directory.GetParent(Application.dataPath).FullName;
		var outputFolder = Path.Combine(projectRoot, "Builds", "Mods");
		return outputFolder;
	}

	static void BuildBundle(string outputFolder, string bundleFileName, string[] assetPaths)
	{
		PathUtils.EnsureDirectoryExists(outputFolder);
		var build = new AssetBundleBuild()
		{
			assetBundleName = bundleFileName,
			assetNames = assetPaths
		};
		var buildParams = new BuildAssetBundlesParameters()
		{
			outputPath = outputFolder,
			bundleDefinitions = new[] { build },
			targetPlatform = EditorUserBuildSettings.activeBuildTarget
		};
		var manifest = BuildPipeline.BuildAssetBundles(buildParams);
	}

	static void DeleteAllManifestFiles(string outputFolder)
	{
		// Delete all .manifest files (recursive)
		foreach (string file in Directory.GetFiles(outputFolder, "*.manifest", SearchOption.TopDirectoryOnly))
		{
			File.Delete(file);
		}

		var modsFile = Path.Combine(outputFolder, "Mods");
		if (File.Exists(modsFile))
		{
			File.Delete(modsFile);
		}
	}
}
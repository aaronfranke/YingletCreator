using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
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

		EditorGUILayout.LabelField("This is the title that will display in-game and on the Steam workshop:");
		EditorGUILayout.PropertyField(_modDisplayTitleProp);

		DrawHorizontalLine(Color.gray);


		EditorGUILayout.LabelField("Click the following button to generate a mod (.bundle) file. This bundle will include:");
		EditorGUILayout.LabelField(" • Presets (.yingsave)");
		EditorGUILayout.LabelField(" • Toggles (CharacterToggleId)");
		EditorGUILayout.LabelField(" • Poses (PoseID)");
		EditorGUILayout.LabelField("Supporting textures, models, and ScriptableObject metadata will also be bundled.");

		var modDefinition = (ModDefinition)target;

		if (GUILayout.Button("Build Mod"))
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
			EditorUtility.DisplayProgressBar("Building mod", $"Assigning assets to appropriate group...", 0.3f);
			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
			var group = GetOrCreateModGroup(addressableSettings, modDefinition);
			var assetPaths = GetAssetPathsForModDefinition(modDefinition);
			MoveAddressablesToGroup(assetPaths, addressableSettings, group);

			EditorUtility.DisplayProgressBar("Building mod", $"Building group...", 0.6f);
			BuildGroup(addressableSettings, group);

			EditorUtility.ClearProgressBar();
			EditorUtility.DisplayDialog("Mod Built", $"Mod contents built to: TBD", "OK");

		}
		catch (Exception ex)
		{
			EditorUtility.ClearProgressBar();
			Debug.LogException(ex);
			EditorUtility.DisplayDialog("Bundle Content", $"An error occurred while bundling: {ex.Message}\nSee Console for details.", "OK");
		}
	}

	static AddressableAssetGroup GetOrCreateModGroup(AddressableAssetSettings addressableSettings, ModDefinition modDefinition)
	{
		string groupName = $"mod-{modDefinition.UniqueAssetID}";

		AddressableAssetGroup targetGroup = addressableSettings.FindGroup(groupName);
		if (targetGroup == null)
		{
			Debug.Log($"Existing group for {groupName} not found. Creating it now...");
			targetGroup = addressableSettings.CreateGroup(
				groupName,
				false, false, false, null,
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema)
			);
		}
		return targetGroup;
	}

	static string[] GetAssetPathsForModDefinition(ModDefinition modDefinition)
	{
		string modAssetPath = AssetDatabase.GetAssetPath(modDefinition);
		var relativeFolder = Path.GetDirectoryName(modAssetPath);

		return GetAssetPathsInFolder(relativeFolder);
	}

	static string[] GetAssetPathsInFolder(string relativeFolder)
	{
		string[] IgnoreExtensions = new[] { ".manifest", "", ModDefinition.ModExtension }; // Don't consume anything generated or any folders

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

	static void MoveAddressablesToGroup(IEnumerable<string> assetRelativePaths, AddressableAssetSettings addressableSettings, AddressableAssetGroup group)
	{
		foreach (string relativePath in assetRelativePaths)
		{
			string guid = AssetDatabase.AssetPathToGUID(relativePath);
			if (string.IsNullOrEmpty(guid))
			{
				Debug.LogWarning($"Could not resolve GUID for: {relativePath}");
				continue;
			}

			var entry = addressableSettings.FindAssetEntry(guid);
			if (entry == null)
			{
				// Any relevant asset should have been made auto addressable by the script
				continue;
			}

			if (entry.parentGroup == group)
			{
				continue; // Already in the appropriate group
			}
			addressableSettings.CreateOrMoveEntry(guid, group);
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	static void BuildGroup(AddressableAssetSettings addressableSettings, AddressableAssetGroup group)
	{
		string buildPath = "../BuiltMods/";
		Directory.CreateDirectory(buildPath);

		var ctx = new AddressablesPlayerBuildResult();
		var buildInput = new AddressablesDataBuilderInput(addressableSettings)
		{

		};

		IDataBuilder builder = addressableSettings.ActivePlayerDataBuilder;
		var result = builder.BuildData<AddressablesPlayerBuildResult>(buildInput);
	}
}
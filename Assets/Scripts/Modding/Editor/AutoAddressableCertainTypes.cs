using Character.Compositor;
using Character.Data;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AutoAddressableCertainTypes : AssetPostprocessor
{
	static Type[] TypesToAutoAddress = new Type[] {
		typeof(CharacterToggleId),
		typeof(CharacterIntId),
		typeof(CharacterSliderId),
		typeof(MixTexture),
		typeof(PoseId),
		typeof(ReColorId),
		typeof(ModDefinition) };

	static void OnPostprocessAllAssets(
		string[] importedAssets,
		string[] deletedAssets,
		string[] movedAssets,
		string[] movedFromAssetPaths)
	{
		AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
		if (settings == null) return;

		foreach (string assetPath in importedAssets)
		{
			var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
			if (asset == null) continue;

			var myType = asset.GetType();
			var firstValidType = TypesToAutoAddress.FirstOrDefault(t => t.IsAssignableFrom(myType));
			if (firstValidType == null) continue;

			string guid = AssetDatabase.AssetPathToGUID(assetPath);
			if (settings.FindAssetEntry(guid) != null) continue; // Already addressable

			var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

			AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, addressableSettings.DefaultGroup, false, false);
			entry.address = entry.guid;
			entry.SetLabel(firstValidType.Name, true);
			settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryCreated, entry, false);

			Debug.Log($"[AutoAddressable] Marked {assetPath} as addressable.");
		}
	}
}

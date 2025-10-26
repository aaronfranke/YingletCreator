using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

/// <summary>
/// By default, addressables use a relative path
/// This is cool if I wasn't planning on reorganizing stuff... but I do, and I don't want to break existing mods
/// So instead, use the asset GUID
/// </summary>
[InitializeOnLoad]
public static class AddressableGUIDAutoSetter
{
	static AddressableGUIDAutoSetter()
	{
		// Hook into Addressable asset changes
		AddressableAssetSettings.OnModificationGlobal += AddressableAssetSettings_OnModificationGlobal;
	}

	private static void AddressableAssetSettings_OnModificationGlobal(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent modEvent, object obj)
	{
		if (modEvent == AddressableAssetSettings.ModificationEvent.EntryAdded)
		{
			if (obj is List<AddressableAssetEntry> entries)
			{
				foreach (var entry in entries)
				{
					if (entry.address != entry.guid)
					{
						entry.SetAddress(entry.guid);
					}
				}
			}
		}
		if (modEvent == AddressableAssetSettings.ModificationEvent.EntryCreated)
		{
			if (obj is AddressableAssetEntry entry)
			{
				if (entry.address != entry.guid)
				{
					entry.SetAddress(entry.guid);
				}
			}
		}
	}
}

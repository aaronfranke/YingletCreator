using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public interface IModLoader
{
	IEnumerable<ModDefinition> AllMods { get; }
}

public class ModLoader : MonoBehaviour, IModLoader
{
	[SerializeField] ModDefinition _builtInMod;

	public IEnumerable<ModDefinition> AllMods { get; private set; }

	private void Awake()
	{
		AllMods = LoadAllModDefinitions();
	}

	IEnumerable<ModDefinition> LoadAllModDefinitions()
	{
		var loadMethod = Singletons.GetSingleton<IResourceLoadMethodProvider>().LoadMethod;
		if (loadMethod == CompositeResourceLoadMethod.EditorAssetLookup)
		{
			// Don't load mods in this format
			return new ModDefinition[0];
		}


		List<ModDefinition> definitions = new();
		definitions.Add(_builtInMod);
		AddModsFolderDefinitions(definitions);
		AddSteamFolderDefinitions(definitions);

		return definitions;
	}

	static void AddModsFolderDefinitions(List<ModDefinition> definitions)
	{
		var modFolder = Singletons.GetSingleton<ISaveFolderProvider>().ModsFolderPath;
		LoadAllFromFolder(definitions, modFolder);
	}


	static void AddSteamFolderDefinitions(List<ModDefinition> definitions)
	{
		var subscribedItems = GetSubscribedItems();
		foreach (var item in subscribedItems)
		{
			LoadAllFromFolder(definitions, item.Directory);
		}
	}
	static void LoadAllFromFolder(List<ModDefinition> definitions, string folder)
	{
		var paths = Directory.GetFiles(folder, $"*{ModDefinition.ModExtension}", SearchOption.AllDirectories);
		foreach (var path in paths)
		{
			var bundle = AssetBundle.LoadFromFile(path);
			var loadedDefinitions = bundle.LoadAllAssets<ModDefinition>();
			if (loadedDefinitions.Length == 0)
			{
				Debug.LogError($"Couldn't find mod definition for {path}, skipping");
				continue;
			}
			if (loadedDefinitions.Length > 1)
			{
				Debug.LogError($"Found more than 1 mod definition for {path}, skipping");
				continue;
			}
			definitions.Add(loadedDefinitions.First());
		}
	}

	static IEnumerable<Steamworks.Ugc.Item> GetSubscribedItems()
	{
		if (!SteamClient.IsValid)
		{
			Debug.LogWarning("Steam is not initialized, so we won't attempt to load workshop mods");
			return Enumerable.Empty<Steamworks.Ugc.Item>();
		}

		// Facepunch SteamWorks is kinda garbage and doesn't actually expose GetNumSubscribedItems or GetSubscribedItems
		// But the internal implementation it uses does...
		// So do some reflection fuckery to get it
		// This is probably very ill-advised but w/e I'm too lazy to configure another steamworks API #yolo
		Type exposedType = typeof(SteamUGC);
		Type internalType = exposedType.Assembly.GetType($"Steamworks.ISteamUGC");
		if (internalType == null)
		{
			Debug.LogWarning("Failed to reflect type, skipping steam mods");
			return Enumerable.Empty<Steamworks.Ugc.Item>();
		}

		// Get the internal static property
		PropertyInfo propertyInfo = exposedType.GetProperty("Internal", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		if (propertyInfo == null)
		{
			Debug.LogWarning("Failed to reflect field, skipping steam mods");
			return Enumerable.Empty<Steamworks.Ugc.Item>();
		}

		object instance = propertyInfo.GetValue(null);
		if (instance == null)
		{
			Debug.LogWarning("Failed to reflect value, skipping steam mods");
			return Enumerable.Empty<Steamworks.Ugc.Item>();
		}

		MethodInfo methodGetNumSubscribedItems = internalType.GetMethod("GetNumSubscribedItems", BindingFlags.NonPublic | BindingFlags.Instance);
		MethodInfo methodGetSubscribedItems = internalType.GetMethod("GetSubscribedItems", BindingFlags.NonPublic | BindingFlags.Instance);
		if (methodGetNumSubscribedItems == null || methodGetSubscribedItems == null)
		{
			Debug.LogWarning("Failed to reflect methods, skipping steam mods");
			return Enumerable.Empty<Steamworks.Ugc.Item>();
		}

		// #1 Call GetNumSubscribed
		var numSubscribed = (uint)(methodGetNumSubscribedItems.Invoke(instance, null));

		// #2 Call GetSubscribedItems
		PublishedFileId[] array = new PublishedFileId[numSubscribed]; // or your desired size
		uint maxEntries = (uint)array.Length;

		object[] parameters = new object[] { array, maxEntries };

		methodGetSubscribedItems.Invoke(instance, parameters);

		// #3 Cast it
		return array.Select(fileId => new Steamworks.Ugc.Item(fileId));
	}
}


using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		var modFolder = Singletons.GetSingleton<ISaveFolderProvider>().ModsFolderPath;
		var paths = Directory.GetFiles(modFolder, $"*{ModDefinition.ModExtension}", SearchOption.AllDirectories);

		List<ModDefinition> definitions = new();
		definitions.Add(_builtInMod);
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
		return definitions;
	}
}

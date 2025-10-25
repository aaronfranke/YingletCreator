using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class provides mods from the resource folder
/// These won't actually appear in game
/// </summary>
public class ResourceProvider_ManuallyAddedMods : MonoBehaviour, IResourceProvider
{
	[SerializeField] bool _logging = true;
	private ISaveFolderProvider _folderProvider;

	void Awake()
	{
		_folderProvider = Singletons.GetSingleton<ISaveFolderProvider>();
	}

	public void IngestContent(CompositeResources compositeResources)
	{
		var folder = Path.Combine(_folderProvider.GameRootFolderPath, "Mods");
		if (!Directory.Exists(folder))
		{
			Directory.CreateDirectory(folder);
		}
		var modFilePaths = Directory.GetFiles(folder, "*" + ModDefinition.ModExtension);

		var assetBundles = modFilePaths.Select(p => AssetBundle.LoadFromFile(p)).ToArray();

		foreach (var assetBundle in assetBundles)
		{
			LoadIntoDictionary(compositeResources.ToggleIds);
			LoadIntoDictionary(compositeResources.RecolorIds);
			LoadIntoDictionary(compositeResources.PoseIds);
			LoadIntoList(compositeResources.MixTextures);

			void LoadIntoDictionary<T>(IDictionary<string, T> dictionary) where T : Object, IHasUniqueAssetId
			{

				var all = assetBundle.LoadAllAssets<T>();
				foreach (var item in all)
				{
					dictionary[item.UniqueAssetID] = item;
				}
				if (_logging && all.Any())
				{
					Debug.Log($"[Manually-Added Mods] Loaded {all.Length} {typeof(T).Name} from mod {assetBundle.name}");
				}
			}

			void LoadIntoList<T>(IList<T> list) where T : Object
			{
				var all = assetBundle.LoadAllAssets<T>();
				foreach (var item in all)
				{
					list.Add(item);
				}
				if (_logging && all.Any())
				{
					Debug.Log($"[Manually-Added Mods] Loaded {all.Length} {typeof(T).Name} from mod {assetBundle.name}");
				}
			}
		}
	}
}

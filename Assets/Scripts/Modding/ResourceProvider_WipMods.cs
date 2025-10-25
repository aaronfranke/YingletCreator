using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;





#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class provides mods from the resource folder
/// These won't actually appear in game
/// </summary>
public class ResourceProvider_WipMods : MonoBehaviour, IResourceProvider
{
#pragma warning disable 0414
	[SerializeField] bool _logging = true;
#pragma warning restore 0414

	public void IngestContent(CompositeResources compositeResources)
	{
#if UNITY_EDITOR
		var resourceMods = Resources.LoadAll<ModDefinition>("");

		foreach (var modDefinition in resourceMods)
		{
			var path = AssetDatabase.GetAssetPath(modDefinition);
			var fullFolderPath = Path.GetDirectoryName(path);
			var folderPath = fullFolderPath.Split("Resources\\")[1];

			LoadIntoDictionary(compositeResources.ToggleIds);
			LoadIntoDictionary(compositeResources.RecolorIds);
			LoadIntoDictionary(compositeResources.PoseIds);
			LoadIntoList(compositeResources.MixTextures);

			void LoadIntoDictionary<T>(IDictionary<string, T> dictionary) where T : Object, IHasUniqueAssetId
			{
				var all = Resources.LoadAll<T>(folderPath);
				foreach (var item in all)
				{
					dictionary[item.UniqueAssetID] = item;
				}
				if (_logging && all.Any())
				{
					Debug.Log($"[WIP Mods] Loaded {all.Length} {typeof(T).Name} from mod {modDefinition.name}");
				}
			}

			void LoadIntoList<T>(IList<T> list) where T : Object
			{
				var all = Resources.LoadAll<T>(folderPath);
				foreach (var item in all)
				{
					list.Add(item);
				}
				if (_logging && all.Any())
				{
					Debug.Log($"[WIP Mods] Loaded {all.Length} {typeof(T).Name} from mod {modDefinition.name}");
				}
			}
		}
#endif
	}
}

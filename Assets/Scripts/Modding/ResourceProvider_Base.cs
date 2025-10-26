using Character.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IResourceProvider
{
	bool enabled { get; }
	void IngestContent(CompositeResources compositeResources);
}

public class ResourceProvider_Base : MonoBehaviour, IResourceProvider
{
	public void IngestContent(CompositeResources compositeResources)
	{
		// Load everything in immediately
		var toggleIds = AddressableExtensionMethods.LoadAllOfTypeSync<CharacterToggleId>().ToArray();
		foreach (var toggle in toggleIds)
		{
			compositeResources.ToggleIds[toggle.UniqueAssetID] = toggle;
		}

		LoadIntoDictionary(compositeResources.ToggleIds);
		LoadIntoDictionary(compositeResources.RecolorIds);
		LoadIntoDictionary(compositeResources.SliderIds);
		LoadIntoDictionary(compositeResources.PoseIds);
		LoadIntoDictionary(compositeResources.IntIds);
		LoadIntoList(compositeResources.MixTextures);
	}


	static void LoadIntoDictionary<T>(IDictionary<string, T> dictionary) where T : Object, IHasUniqueAssetId
	{
		var folder = typeof(T).Name;
		var all = Resources.LoadAll<T>(folder);
		foreach (var item in all)
		{
			dictionary[item.UniqueAssetID] = item;
		}
	}

	static void LoadIntoList<T>(IList<T> list) where T : Object
	{
		var folder = typeof(T).Name;
		var all = Resources.LoadAll<T>(folder);
		foreach (var item in all)
		{
			list.Add(item);
		}
	}
}

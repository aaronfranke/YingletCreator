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
		///Addressables.InitializeAsync().WaitForCompletion();

		LoadIntoDictionary(compositeResources.ToggleIds);
		Debug.LogWarning($"Loaded {compositeResources.ToggleIds.Count()} toggles");

		LoadIntoDictionary(compositeResources.RecolorIds);
		Debug.LogWarning($"Loaded {compositeResources.RecolorIds.Count()} RecolorIds");

		LoadIntoDictionary(compositeResources.SliderIds);
		Debug.LogWarning($"Loaded {compositeResources.SliderIds.Count()} SliderIds");

		LoadIntoDictionary(compositeResources.PoseIds);
		Debug.LogWarning($"Loaded {compositeResources.PoseIds.Count()} PoseIds");

		LoadIntoDictionary(compositeResources.IntIds);
		Debug.LogWarning($"Loaded {compositeResources.IntIds.Count()} toggIntIdsles");

		LoadIntoList(compositeResources.MixTextures);
		Debug.LogWarning($"Loaded {compositeResources.MixTextures.Count()} MixTextures");
	}


	static void LoadIntoDictionary<T>(IDictionary<string, T> dictionary) where T : Object, IHasUniqueAssetId
	{
		var items = AddressableExtensionMethods.LoadAllOfTypeSync<T>().ToArray();
		foreach (var item in items)
		{
			dictionary[item.UniqueAssetID] = item;
		}
	}

	static void LoadIntoList<T>(IList<T> list) where T : Object
	{
		var items = AddressableExtensionMethods.LoadAllOfTypeSync<T>().ToArray();
		foreach (var item in items)
		{
			list.Add(item);
		}
	}
}

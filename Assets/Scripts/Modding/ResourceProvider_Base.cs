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
		LoadIntoDictionary(compositeResources.RecolorIds);
		LoadIntoDictionary(compositeResources.SliderIds);
		LoadIntoDictionary(compositeResources.PoseIds);
		LoadIntoDictionary(compositeResources.IntIds);
		LoadIntoList(compositeResources.MixTextures);
	}


	static void LoadIntoDictionary<T>(IDictionary<string, T> dictionary) where T : Object, IHasUniqueAssetId
	{
		var items = AssetReferenceT<T>.LoadAllOfTypeSync().ToArray();
		foreach (var item in items)
		{
			dictionary[item.UniqueAssetID] = item;
		}
	}

	static void LoadIntoList<T>(IList<T> list) where T : Object
	{
		var items = AssetReferenceT<T>.LoadAllOfTypeSync().ToArray();
		foreach (var item in items)
		{
			list.Add(item);
		}
	}
}

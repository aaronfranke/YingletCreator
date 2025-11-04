
using Character.Compositor;
using Character.Data;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Loads resources combining:
/// - Pre-existing resources defined in the resource folder
/// - WIP mods, exposed via the editor resource folder
/// - Mods manually installed in the Yinglet Creator folder
/// - Mods installed via the steam workshop
/// </summary>
/// 
public interface ICompositeResourceLoader
{
	CharacterToggleId LoadCharacterToggleId(string uniqueAssetId);
	ReColorId LoadReColorId(string uniqueAssetId);
	CharacterSliderId LoadCharacterSliderId(string uniqueAssetId);
	CharacterIntId LoadCharacterIntId(string uniqueAssetId);
	PoseId LoadPoseId(string uniqueAssetId);

	IEnumerable<CharacterToggleId> LoadAllToggleIds();
	IEnumerable<PoseId> LoadAllPoseIds();
	IEnumerable<MixTexture> LoadMixTextures();
}


public sealed class CompositeResources
{
	public Dictionary<string, CharacterToggleId> ToggleIds { get; } = new();
	public Dictionary<string, ReColorId> RecolorIds { get; } = new();
	public Dictionary<string, CharacterSliderId> SliderIds { get; } = new();
	public Dictionary<string, PoseId> PoseIds { get; } = new();
	public Dictionary<string, CharacterIntId> IntIds { get; } = new();
	public List<MixTexture> MixTextures { get; } = new();
}

public class CompositeResourceLoader : MonoBehaviour, ICompositeResourceLoader
{
	private CompositeResources _compositeResources;

	private void Awake()
	{
		_compositeResources = new CompositeResources();

		var resourceProviders = this.GetComponentsInChildren<IResourceProvider>();

		foreach (var resourceProvider in resourceProviders)
		{
			if (!resourceProvider.enabled) continue;
			resourceProvider.IngestContent(_compositeResources);
		}
	}

	public CharacterIntId LoadCharacterIntId(string uniqueAssetId)
	{
		return _compositeResources.IntIds[uniqueAssetId];
	}

	public CharacterSliderId LoadCharacterSliderId(string uniqueAssetId)
	{
		return _compositeResources.SliderIds[uniqueAssetId];
	}

	public CharacterToggleId LoadCharacterToggleId(string uniqueAssetId)
	{
		return _compositeResources.ToggleIds[uniqueAssetId];
	}

	public PoseId LoadPoseId(string uniqueAssetId)
	{
		return _compositeResources.PoseIds[uniqueAssetId];
	}

	public ReColorId LoadReColorId(string uniqueAssetId)
	{
		return _compositeResources.RecolorIds[uniqueAssetId];
	}

	public IEnumerable<PoseId> LoadAllPoseIds()
	{
		return _compositeResources.PoseIds.Values;
	}
	public IEnumerable<CharacterToggleId> LoadAllToggleIds()
	{
		return _compositeResources.ToggleIds.Values;
	}

	public IEnumerable<MixTexture> LoadMixTextures()
	{
		return _compositeResources.MixTextures;
	}
}

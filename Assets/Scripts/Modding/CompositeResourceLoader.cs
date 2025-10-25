
using Character.Compositor;
using Character.Data;
using System.Collections.Generic;
using System.Linq;
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

public class CompositeResourceLoader : MonoBehaviour, ICompositeResourceLoader
{
	IDictionary<string, CharacterToggleId> _toggleIdCache;
	IDictionary<string, ReColorId> _recolorIdCache;
	IDictionary<string, CharacterSliderId> _sliderIdCache;
	IDictionary<string, PoseId> _poseIdCache;
	IDictionary<string, CharacterIntId> _intIdCache;
	IEnumerable<MixTexture> _mixTextureCache;

	private void Awake()
	{
		// Load everything in immediately
		_toggleIdCache = LoadIntoDictionary<CharacterToggleId>();
		_recolorIdCache = LoadIntoDictionary<ReColorId>();
		_sliderIdCache = LoadIntoDictionary<CharacterSliderId>();
		_poseIdCache = LoadIntoDictionary<PoseId>();
		_intIdCache = LoadIntoDictionary<CharacterIntId>();
		_mixTextureCache = LoadIntoEnumerable<MixTexture>();
	}

	public CharacterIntId LoadCharacterIntId(string uniqueAssetId)
	{
		return _intIdCache[uniqueAssetId];
	}

	public CharacterSliderId LoadCharacterSliderId(string uniqueAssetId)
	{
		return _sliderIdCache[uniqueAssetId];
	}

	public CharacterToggleId LoadCharacterToggleId(string uniqueAssetId)
	{
		return _toggleIdCache[uniqueAssetId];
	}

	public PoseId LoadPoseId(string uniqueAssetId)
	{
		return _poseIdCache[uniqueAssetId];
	}

	public ReColorId LoadReColorId(string uniqueAssetId)
	{
		return _recolorIdCache[uniqueAssetId];
	}

	public IEnumerable<PoseId> LoadAllPoseIds()
	{
		return _poseIdCache.Values;
	}
	public IEnumerable<CharacterToggleId> LoadAllToggleIds()
	{
		return _toggleIdCache.Values;
	}

	public IEnumerable<MixTexture> LoadMixTextures()
	{
		return _mixTextureCache;
	}


	static IDictionary<string, T> LoadIntoDictionary<T>() where T : Object, IHasUniqueAssetId
	{
		var folder = typeof(T).Name;
		var all = Resources.LoadAll<T>(folder);
		return all.ToDictionary(t => t.UniqueAssetID, t => t);
	}

	static IEnumerable<T> LoadIntoEnumerable<T>() where T : Object
	{
		var folder = typeof(T).Name;
		var all = Resources.LoadAll<T>(folder);
		return all;
	}
}

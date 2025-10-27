using Character.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Compositor
{

	public interface IMixTexture : ITaggableCharacterElement
	{

		/// <summary>
		/// The ID used to re-color this
		/// If null, this mix texture can't be recolored
		/// </summary>
		ReColorId ReColorId { get; }

		/// <summary>
		/// What material this texture should be applied to
		/// </summary>
		MaterialDescription TargetMaterialDescription { get; }

		/// <summary>
		/// Texture that contains the shading for this. Usually a reddish gradient 
		/// </summary>
		Texture2D Grayscale { get; }

		/// <summary>
		/// Optional texture that controls what parts of the texture should be rendered
		/// </summary>
		Texture2D Mask { get; }

		string name { get; }

		bool Sortable { get; }

		TargetMaterialTexture TargetMaterialTexture { get; }
	}

	[CreateAssetMenu(fileName = "MixTexture", menuName = "Scriptable Objects/Character Compositor/MixTexture")]
	public class MixTexture : ScriptableObject, IMixTexture, IOrderableScriptableObject<MixTextureOrderGroup>
	{
		[SerializeField] ReColorId _reColorId;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _grayscale;
		[SerializeField] Texture2D _mask;
		[SerializeField] CharacterElementTag[] _tags;
		[SerializeField] MixTextureOrderData _order;

		public ReColorId ReColorId => _reColorId;

		public MaterialDescription TargetMaterialDescription => _targetMaterialDescription;

		public Texture2D Grayscale => _grayscale;

		public Texture2D Mask => _mask;

		public bool Sortable => true;

		public virtual TargetMaterialTexture TargetMaterialTexture => TargetMaterialTexture.MainTexture;

		public IEnumerable<CharacterElementTag> Tags => _tags;

		public MixTextureOrderData Order => _order;
		IOrderData<MixTextureOrderGroup> IOrderableScriptableObject<MixTextureOrderGroup>.Order => Order;
	}

	public enum TargetMaterialTexture
	{
		MainTexture,
		Eyelid, // Used in the eye material; covers the pupil
		Pupil, // Used in the eye material
		Mouth, // Used in the mouth material; includes line and teeth
		MouthMask, // Used in the mouth material; the center that designates what should be alpha clipped out
	}

	[System.Serializable]
	public class MixTextureOrderData : IOrderData<MixTextureOrderGroup>
	{
		[SerializeField] AssetReferenceT<MixTextureOrderGroup> _groupReference;
		public MixTextureOrderGroup Group => _groupReference.LoadSync();

		[SerializeField] int _index;
		public int Index => _index;
	}
}

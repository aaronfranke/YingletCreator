using Character.Data;
using System.Collections.Generic;
using UnityEngine;

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
	public class MixTexture : ScriptableObject, IMixTexture
	{
		[SerializeField] ReColorId _reColorId;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _grayscale;
		[SerializeField] Texture2D _mask;
		[SerializeField] CharacterElementTag[] _tags;
		[SerializeField] CharacterElementTag[] _maskedTags;

		public ReColorId ReColorId => _reColorId;

		public MaterialDescription TargetMaterialDescription => _targetMaterialDescription;

		public Texture2D Grayscale => _grayscale;

		public Texture2D Mask => _mask;

		public bool Sortable => true;

		public TargetMaterialTexture TargetMaterialTexture => TargetMaterialTexture.MainTexture;

		public IEnumerable<CharacterElementTag> Tags => _tags;
		public IEnumerable<CharacterElementTag> MaskedTags => _maskedTags;
	}

	public enum TargetMaterialTexture
	{
		MainTexture,
		Outline, // Used in the eye texture; covers the pupil
		Pupil // Used in the eye texture
	}
}

using Character.Data;
using UnityEngine;

namespace Character.Compositor
{

	public interface IMixTexture
	{

		/// <summary>
		/// A grouping that determines the default color for this texture if a recolor isn't provided
		/// Additionally, if a new item isn't recolored but an item in the group is,
		/// This will use that recolored item's texture recolor
		/// </summary>
		ColorGroup DefaultColorGroup { get; }

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
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField] ReColorId _reColorId;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _grayscale;
		[SerializeField] Texture2D _mask;

		public ColorGroup DefaultColorGroup => _defaultColorGroup;

		public ReColorId ReColorId => _reColorId;

		public MaterialDescription TargetMaterialDescription => _targetMaterialDescription;

		public Texture2D Grayscale => _grayscale;

		public Texture2D Mask => _mask;

		public bool Sortable => true;

		public TargetMaterialTexture TargetMaterialTexture => TargetMaterialTexture.MainTexture;

	}

	public enum TargetMaterialTexture
	{
		MainTexture,
		Outline, // Used in the eye texture; covers the pupil
		Pupil // Used in the eye texture
	}
}

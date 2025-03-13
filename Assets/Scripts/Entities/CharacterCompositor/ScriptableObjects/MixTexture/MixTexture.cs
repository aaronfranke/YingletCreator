using UnityEngine;

namespace CharacterCompositor
{

	public interface IMixTexture {
		
		/// <summary>
		/// Textures are grouped together to share the same color adjustments
		/// This property determines which group this texture should default into
		/// </summary>
		ColorGroup DefaultColorGroup { get; }

		/// <summary>
		/// When Contrast=0, this is the color that should be used for this texture
		/// Generally an average between the high and low colors
		/// </summary>
		Color ContrastMidpointColor { get; }
		
		/// <summary>
		/// What material this texture should be applied to
		/// </summary>
		MaterialDescription TargetMaterialDescription  { get; }
		
		/// <summary>
		/// Texture that contains the shading for this. Usually a reddish gradient 
		/// </summary>
		Texture2D Shading  { get; }

		/// <summary>
		/// Optional texture that controls what parts of the texture should be rendered
		/// </summary>
		Texture2D Mask  { get; }

		string name { get; }

		bool Sortable { get; }
	}

	[CreateAssetMenu(fileName = "MixTexture", menuName = "Scriptable Objects/Character Compositor/MixTexture")]
	public class MixTexture : ScriptableObject, IMixTexture
	{
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField][ColorUsage(false)] Color _contrastMidpointColor = Color.gray;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _shading;
		[SerializeField] Texture2D _mask;

		public ColorGroup DefaultColorGroup => _defaultColorGroup;

		public Color ContrastMidpointColor => _contrastMidpointColor;
		
		public MaterialDescription TargetMaterialDescription => _targetMaterialDescription;
		
		public Texture2D Shading => _shading;

		public Texture2D Mask => _mask;

		public bool Sortable => true;
    }
}

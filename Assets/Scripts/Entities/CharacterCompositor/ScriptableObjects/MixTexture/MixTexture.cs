using UnityEngine;

namespace CharacterCompositor
{


	[CreateAssetMenu(fileName = "MixTexture", menuName = "Scriptable Objects/Character Compositor/MixTexture")]
	public class MixTexture : ScriptableObject
	{
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField][ColorUsage(false)] Color _contrastMidpointColor = Color.gray;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _shading;
		[SerializeField] Texture2D _mask;

		/// <summary>
		/// Textures are grouped together to share the same color adjustments
		/// This property determines which group this texture should default into
		/// </summary>
		public ColorGroup DefaultColorGroup => _defaultColorGroup;

		/// <summary>
		/// When Contrast=0, this is the color that should be used for this texture
		/// Generally an average between the high and low colors
		/// </summary>
		public Color ContrastMidpointColor => _contrastMidpointColor;
		
		/// <summary>
		/// What material this texture should be applied to
		/// </summary>
		public MaterialDescription TargetMaterialDescription => _targetMaterialDescription;
		
		/// <summary>
		/// Texture that contains the shading for this. Usually a reddish gradient 
		/// </summary>
		public Texture2D Shading => _shading;

		/// <summary>
		/// Optional texture that controls 
		/// </summary>
		public Texture2D Mask => _mask;
	}
}

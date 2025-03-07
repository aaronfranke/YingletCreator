using UnityEngine;

namespace CharacterCompositor
{


	[CreateAssetMenu(fileName = "MixTexture", menuName = "Scriptable Objects/Character Compositor/MixTexture")]
	public class MixTexture : ScriptableObject
	{
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField] MaterialDescription _targetMaterialDescription;
		[SerializeField] Texture2D _shading;
		[SerializeField] Texture2D _mask;
		// TODO: Also add something to control sorting probly

		public ColorGroup DefaultColorGroup => _defaultColorGroup;
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

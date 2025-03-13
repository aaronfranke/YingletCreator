using UnityEngine;

namespace CharacterCompositor
{


	[CreateAssetMenu(fileName = "MixTexture", menuName = "Scriptable Objects/Character Compositor/MixTexture (Property Targeting)")]
	public class PropertyTargetingMixTexture : MixTexture
	{
		[SerializeField] string _materialPropertyName;

		public string MaterialPropertyName => _materialPropertyName;
	}
}

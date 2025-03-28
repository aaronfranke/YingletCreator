using System.Collections;
using CharacterCompositor;
using UnityEngine;

namespace CharacterCompositor
{

	[CreateAssetMenu(fileName = "MixTextureOrdering", menuName = "Scriptable Objects/Character Compositor/MixTextureOrdering")]
	public class MixTextureOrdering : ScriptableObject
	{
		[SerializeField] MixTexture[] _mixTextures;

		public MixTexture[] OrderedMixTextures => _mixTextures;
	}
}

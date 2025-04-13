using System.Collections;
using Character.Compositor;
using UnityEngine;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MixTextureOrdering", menuName = "Scriptable Objects/Character Compositor/MixTextureOrdering")]
	public class MixTextureOrdering : ScriptableObject
	{
		[SerializeField] MixTexture[] _mixTextures;

		public MixTexture[] OrderedMixTextures => _mixTextures;
	}
}

using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MixTextureOrdering", menuName = "Scriptable Objects/Character Compositor/MixTextureOrdering")]
	public class MixTextureOrdering : ScriptableObject
	{
		[SerializeField] MixTextureOrderGroup_Old[] _groups;

		public IEnumerable<MixTexture> OrderedMixTextures
		{
			get
			{
				foreach (var group in _groups)
				{
					foreach (var tex in group.MixTextures)
					{
						yield return tex;
					}
				}
			}
		}
	}
	[System.Serializable]
	public class MixTextureOrderGroup_Old
	{
		[SerializeField] string _name;
		[SerializeField] MixTexture[] _mixTextures;

		public IEnumerable<MixTexture> MixTextures => _mixTextures;
	}
}

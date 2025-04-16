using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public class TextureGatherer_StaticAdditions : MonoBehaviour, ITextureGathererMutator
	{
		[SerializeField] MixTexture[] _textures;
		public void Mutate(ref ISet<IMixTexture> set)
		{
			foreach (var tex in _textures)
			{
				set.Add(tex);
			}
		}
	}
}
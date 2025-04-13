using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public class TextureGatherer_StaticAdditions : MonoBehaviour, ITextureGathererMutator
	{
		[SerializeField] MixTexture[] _textures;
		public void Mutate(ref ISet<MixTexture> set)
		{
			foreach (var mesh in _textures)
			{
				set.Add(mesh);
			}
		}
	}
}
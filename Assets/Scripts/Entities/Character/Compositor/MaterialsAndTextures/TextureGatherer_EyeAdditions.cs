using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public class TextureGatherer_EyeAdditions : MonoBehaviour, ITextureGathererMutator
	{
		[SerializeField] EyeMixTextureReferences _eyeMixTextureReferences;
		[SerializeField] EyeMixTextures _eyeMixTexture;

		// If the reference ever changes (which it will) we'll need to make this observable
		private IMixTexture[] _cachedMixTextures;

		void Awake()
		{
			_cachedMixTextures = _eyeMixTexture.GenerateMixTextures(_eyeMixTextureReferences).ToArray();
		}
		public void Mutate(ref ISet<IMixTexture> set)
		{
			foreach (var tex in _cachedMixTextures)
			{
				set.Add(tex);
			}
		}
	}
}
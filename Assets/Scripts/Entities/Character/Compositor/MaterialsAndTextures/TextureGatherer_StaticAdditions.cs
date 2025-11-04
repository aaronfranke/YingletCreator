using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Character.Compositor
{
	public class TextureGatherer_StaticAdditions : MonoBehaviour, ITextureGathererMutator
	{
		[SerializeField] AssetReferenceT<MixTexture>[] _textureReferences;
		private MixTexture[] _textures;

		MixTexture[] Textures
		{
			get
			{
				if (_textures == null)
				{
					_textures = _textureReferences.Select(r => r.LoadSync()).ToArray();
				}
				return _textures;
			}
		}

		public void Mutate(ref ISet<IMixTexture> set)
		{
			foreach (var tex in Textures)
			{
				set.Add(tex);
			}
		}
	}
}
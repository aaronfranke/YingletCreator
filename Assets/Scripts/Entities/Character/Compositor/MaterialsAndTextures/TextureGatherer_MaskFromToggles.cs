using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public class TextureGatherer_MaskFromToggles : MonoBehaviour, ITextureGathererMutator
	{
		private IYingletMaskedTagsProvider _maskedTagsProvider;

		void Awake()
		{
			_maskedTagsProvider = this.GetComponentInParent<IYingletMaskedTagsProvider>();
		}

		public void Mutate(ref ISet<IMixTexture> set)
		{
			var tags = _maskedTagsProvider.MaskedTags.ToArray();

			var toRemove = new List<IMixTexture>();
			foreach (var texture in set)
			{
				if (texture.Tags.Any(tag => tags.Contains(tag)))
				{
					toRemove.Add(texture);
				}
			}

			foreach (var texture in toRemove)
			{
				set.Remove(texture);
			}
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public class MeshGatherer_MaskFromToggles : MonoBehaviour, IMeshGathererMutator
	{
		private IYingletMaskedTagsProvider _maskedTagsProvider;

		void Awake()
		{
			_maskedTagsProvider = this.GetComponentInParent<IYingletMaskedTagsProvider>();
		}

		public void Mutate(ref ISet<MeshWithMaterial> set)
		{
			var tags = _maskedTagsProvider.MaskedTags.ToArray();

			var toRemove = new List<MeshWithMaterial>();
			foreach (var mesh in set)
			{
				if (mesh.Tags == null) continue;
				if (mesh.Tags.Any(tag => tags.Contains(tag)))
				{
					toRemove.Add(mesh);
				}
			}

			foreach (var mesh in toRemove)
			{
				set.Remove(mesh);
			}
		}
	}
}
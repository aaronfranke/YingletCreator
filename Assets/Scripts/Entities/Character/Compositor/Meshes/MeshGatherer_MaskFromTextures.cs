using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public class MeshGatherer_MaskFromTextures : ReactiveBehaviour, IMeshGathererMutator
	{
		private ITextureGatherer _textureGatherer;
		EnumerableSetReflector<MeshTag> _maskedTags = new();


		void Awake()
		{
			_textureGatherer = this.GetCompositedYingletComponent<ITextureGatherer>();
			AddReflector(ComputeMaskedTags);
		}

		private void ComputeMaskedTags()
		{
			var set = new HashSet<MeshTag>();
			var relevantTextures = _textureGatherer.AllRelevantTextures.ToList();
			foreach (var relevantTexture in relevantTextures)
			{
				foreach (var maskTag in relevantTexture.MaskedMeshTags)
				{
					set.Add(maskTag);
				}
			}
			_maskedTags.Enumerate(set);
		}

		public void Mutate(ref ISet<MeshWithMaterial> set)
		{
			var tags = _maskedTags.Items.ToHashSet();

			var toRemove = new List<MeshWithMaterial>();
			foreach (var mesh in set)
			{
				if (mesh.Tags.Any(tag => tags.Contains(tag)))
				{
					toRemove.Add(mesh);
				}
			}

			Debug.Log("Removing: " + string.Join(", ", toRemove.Select(t => t.name)));
			foreach (var mesh in toRemove)
			{
				set.Remove(mesh);
			}
		}
	}
}
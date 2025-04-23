using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public class MeshGatherer_MaskFromMeshes : MonoBehaviour, IMeshGathererMutator
	{
		public void Mutate(ref ISet<MeshWithMaterial> set)
		{
			var maskedTags = new HashSet<MeshTag>();
			foreach (var mesh in set)
			{
				foreach (var tag in mesh.MaskedTags)
				{
					maskedTags.Add(tag);
				}
			}

			var toRemove = new List<MeshWithMaterial>();
			foreach (var mesh in set)
			{
				if (mesh.Tags.Any(tag => maskedTags.Contains(tag)))
				{
					toRemove.Add(mesh);
				}
			}

			// Debug.Log("Removing: " + string.Join(", ", toRemove.Select(t => t.name)));
			foreach (var mesh in toRemove)
			{
				set.Remove(mesh);
			}
		}
	}
}
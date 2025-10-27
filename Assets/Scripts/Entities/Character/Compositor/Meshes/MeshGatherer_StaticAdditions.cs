using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public class MeshGatherer_StaticAdditions : MonoBehaviour, IMeshGathererMutator
	{
		[SerializeField] MeshWithMaterial[] _meshes;  // TTODO
		public void Mutate(ref ISet<MeshWithMaterial> set)
		{
			foreach (var mesh in _meshes)
			{
				set.Add(mesh);
			}
		}
	}
}
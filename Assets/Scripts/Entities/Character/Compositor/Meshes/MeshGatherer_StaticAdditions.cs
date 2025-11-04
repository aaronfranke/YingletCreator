using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Character.Compositor
{
	public class MeshGatherer_StaticAdditions : MonoBehaviour, IMeshGathererMutator
	{
		[SerializeField] AssetReferenceT<MeshWithMaterial>[] _meshReferences;
		public void Mutate(ref ISet<MeshWithMaterial> set)
		{
			foreach (var mesh in _meshReferences.Select(m => m.LoadSync()))
			{
				set.Add(mesh);
			}
		}
	}
}
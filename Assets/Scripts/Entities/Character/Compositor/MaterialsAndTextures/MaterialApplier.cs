using Reactivity;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public class MaterialApplier : ReactiveBehaviour
	{
		private IMeshGeneration _meshGeneration;
		private IReadOnlyDictionary<MaterialDescription, Material> _lookup;
		private EnumerableSetReflector<MeshObjectWithMaterialDescription> _enumerableReflector;

		private void Awake()
		{
			_meshGeneration = this.GetCompositedYingletComponent<IMeshGeneration>();
			var materialGeneration = this.GetComponent<IMaterialGeneration>();
			_lookup = materialGeneration.GeneratedMaterialLookup; // Grabbing this reference outside the reactive scope is not ideal, but should be fiiine and save us a bit of headache
			_enumerableReflector = new(Create);
			AddReflector(Reflect);
		}

		private void Create(MeshObjectWithMaterialDescription description)
		{
			if (_lookup.TryGetValue(description.MaterialDescription, out Material material))
			{
				description.MeshGO.GetComponent<SkinnedMeshRenderer>().material = material;
				// Debug.Log($"Applied material description {description.MaterialDescription.name} to mesh object {description.MeshGO.name}");
			}
			else
			{
				Debug.LogWarning($"Failed to find a material with description {description.MaterialDescription.name} for mesh object {description.MeshGO.name}");
			}
		}

		private void Reflect()
		{
			_enumerableReflector.Enumerate(_meshGeneration.Meshes);

		}
	}

}
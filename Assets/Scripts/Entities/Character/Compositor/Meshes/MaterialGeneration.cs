using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public interface IMaterialGeneration
	{
		IReadOnlyDictionary<MaterialDescription, Material> GeneratedMaterialLookup { get; }
	}

	/// <summary>
	/// Reads MaterialDescriptions from MeshDefinition
	/// Generates actual materials and exposes them
	/// Does not apply them
	/// </summary>
	public class MaterialGeneration : ReactiveBehaviour, IMaterialGeneration
	{

		private EnumerableReflector<MaterialDescription, Material> _enumerableReflector;
		private IMeshDefinition _meshDefinition;

		public IReadOnlyDictionary<MaterialDescription, Material> GeneratedMaterialLookup => _enumerableReflector.Dict;

		private void Awake()
		{
			_meshDefinition = this.GetComponent<IMeshDefinition>();
			_enumerableReflector = new(Create, Delete);
			AddReflector(Composite);
		}

		private Material Create(MaterialDescription description)
		{
			return new Material(description.ReferenceMaterial);
		}
		private void Delete(Material material)
		{
			Destroy(material);
		}

		public void Composite()
		{
			var materials = _meshDefinition.DefinedMeshes.Select(m => m.MaterialDescription).ToHashSet();
			_enumerableReflector.Enumerate(materials);
		}
	}
}
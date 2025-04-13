using Reactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public interface IMaterialGeneration
	{
		IReadOnlyDictionary<MaterialDescription, Material> GeneratedMaterialLookup { get; }
		IEnumerable<MaterialWithDescription> GeneratedMaterialsWithDescription { get; }
	}

	/// <summary>
	/// Reads MaterialDescriptions from MeshDefinition
	/// Generates actual materials and exposes them
	/// Does not apply them
	/// </summary>
	public class MaterialGeneration : ReactiveBehaviour, IMaterialGeneration
	{

		private EnumerableReflector<MaterialDescription, Material> _enumerableReflector;
		private IMeshGatherer _meshDefinition;

		public IReadOnlyDictionary<MaterialDescription, Material> GeneratedMaterialLookup => _enumerableReflector.Dict;

		public IEnumerable<MaterialWithDescription> GeneratedMaterialsWithDescription
		{
			get
			{
				return _enumerableReflector.KVP.Select(kvp => new MaterialWithDescription(kvp.Value, kvp.Key));
			}
		}

		private void Awake()
		{
			_meshDefinition = this.GetCompositedYingletComponent<IMeshGatherer>();
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
			var materials = _meshDefinition.AllRelevantMeshes.Select(m => m.MaterialDescription).ToHashSet();
			_enumerableReflector.Enumerate(materials);
		}
	}

	public class MaterialWithDescription
	{
		public MaterialWithDescription(Material material, MaterialDescription description)
		{
			Material = material;
			Description = description;
		}

		public Material Material { get; }
		public MaterialDescription Description { get; }
		public override bool Equals(object obj)
		{
			return obj is MaterialWithDescription other
				&& Material == other.Material
				&& Description == other.Description;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Material, Description);
		}
	}
}
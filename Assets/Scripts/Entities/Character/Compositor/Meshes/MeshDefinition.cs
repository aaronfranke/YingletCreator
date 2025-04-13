using Reactivity;
using System.Collections.Generic;

namespace Character.Compositor
{
	/// <summary>
	/// Reads the input customization data and generates the set of meshess that need to be created
	/// Does not actually generate any meshes based off of this information
	/// </summary>
	public interface IMeshDefinition
	{
		/// <summary>
		/// The set of meshes defined
		/// A corresponding GameObject should be created for all of these
		/// </summary>
		IEnumerable<MeshWithMaterial> DefinedMeshes { get; }
	}
	public class MeshDefinition : ReactiveBehaviour, IMeshDefinition
	{
		private IMeshDefinitionMutator[] _mutators;
		private EnumerableReflector<MeshWithMaterial, object> _enumerableReflector;

		public IEnumerable<MeshWithMaterial> DefinedMeshes => _enumerableReflector.Keys;

		void Awake()
		{
			_mutators = this.GetComponentsInChildren<IMeshDefinitionMutator>();
			_enumerableReflector = new EnumerableReflector<MeshWithMaterial, object>(Added, Removed);
			AddReflector(Composite);
		}
		private object Added(MeshWithMaterial material) => null;
		private void Removed(object obj) { }

		public void Composite()
		{
			ISet<MeshWithMaterial> set = new HashSet<MeshWithMaterial>();
			foreach (var m in _mutators)
			{
				m.Mutate(ref set);
			}
			_enumerableReflector.Enumerate(set);
		}
	}

	/// <summary>
	/// Interface that can either add or remove meshes from the set
	/// </summary>
	public interface IMeshDefinitionMutator
	{
		/// <summary>
		/// Reflectively called, giving the implementation a chance to add or remove meshes from the list
		/// </summary>
		public void Mutate(ref ISet<MeshWithMaterial> meshes);
	}
}

using Reactivity;
using System.Collections.Generic;

namespace Character.Compositor
{
	/// <summary>
	/// Reads the input customization data and generates the set of meshess that need to be created
	/// Does not actually generate any meshes based off of this information
	/// </summary>
	public interface IMeshGatherer
	{
		/// <summary>
		/// The set of all meshes relevant to this composition
		/// A corresponding GameObject should be created for all of these
		/// </summary>
		IEnumerable<MeshWithMaterial> AllRelevantMeshes { get; }
	}
	public class MeshGatherer : ReactiveBehaviour, IMeshGatherer
	{
		private IMeshGathererMutator[] _mutators;
		private EnumerableSetReflector<MeshWithMaterial> _enumerableReflector;

		public IEnumerable<MeshWithMaterial> AllRelevantMeshes => _enumerableReflector.Items;

		void Awake()
		{
			_mutators = this.GetComponentsInChildren<IMeshGathererMutator>();
			_enumerableReflector = new();
			AddReflector(Composite);
		}

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
	public interface IMeshGathererMutator
	{
		/// <summary>
		/// Reflectively called, giving the implementation a chance to add or remove meshes from the list
		/// </summary>
		public void Mutate(ref ISet<MeshWithMaterial> meshes);
	}
}

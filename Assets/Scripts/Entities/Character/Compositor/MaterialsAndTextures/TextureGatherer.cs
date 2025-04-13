using Reactivity;
using System.Collections.Generic;

namespace Character.Compositor
{
	public interface ITextureGatherer
	{
		IEnumerable<IMixTexture> AllRelevantTextures { get; }

	}

	public class TextureGatherer : ReactiveBehaviour, ITextureGatherer
	{
		private ITextureGathererMutator[] _mutators;
		private EnumerableDictReflector<IMixTexture, object> _enumerableReflector;

		public IEnumerable<IMixTexture> AllRelevantTextures => _enumerableReflector.Keys;

		void Awake()
		{
			_mutators = this.GetComponentsInChildren<ITextureGathererMutator>();
			_enumerableReflector = new EnumerableDictReflector<IMixTexture, object>(Added, Removed);
			AddReflector(Composite);
		}
		private object Added(IMixTexture material) => null;
		private void Removed(object obj) { }

		public void Composite()
		{
			ISet<IMixTexture> set = new HashSet<IMixTexture>();
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
	public interface ITextureGathererMutator
	{
		/// <summary>
		/// Reflectively called, giving the implementation a chance to add or remove meshes from the list
		/// </summary>
		public void Mutate(ref ISet<IMixTexture> textures);
	}
}
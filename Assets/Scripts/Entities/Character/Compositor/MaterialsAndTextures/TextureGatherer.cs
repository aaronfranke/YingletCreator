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
		private EnumerableSetReflector<IMixTexture> _enumerableReflector;

		public IEnumerable<IMixTexture> AllRelevantTextures => _enumerableReflector.Items;

		void Awake()
		{
			_mutators = this.GetComponentsInChildren<ITextureGathererMutator>();
			_enumerableReflector = new();
			AddReflector(Composite);
		}

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
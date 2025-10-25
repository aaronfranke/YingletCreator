using Character.Compositor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IMixTextureOrderer
{
	IEnumerable<MixTexture> GetOrderedMixTextures();
}

public class MixTextureOrderer : MonoBehaviour, IMixTextureOrderer
{
	[SerializeField] MixTextureOrderGroup[] _orderGroups;
	IEnumerable<MixTexture> _orderedMixTextures;

	private void Awake()
	{
		var resourceLoader = Singletons.GetSingleton<ICompositeResourceLoader>();

		List<MixTexture> _ordered = new();
		var allMixTextures = resourceLoader.LoadMixTextures();
		foreach (var group in _orderGroups)
		{
			var groupMixTextures = allMixTextures
				.Where(t => t.Order.Group == group)
				.OrderBy(t => t.Order.Index);
			_ordered.AddRange(groupMixTextures);
		}
		_orderedMixTextures = _ordered;
	}

	public IEnumerable<MixTexture> GetOrderedMixTextures()
	{
		return _orderedMixTextures;
	}
}

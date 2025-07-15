using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	/// <summary>
	/// This script formerly housed every MixTexture
	/// However, having to go and update it every time a MixTexture was added was tedious
	/// Instead, each MixTexture now mantains its own group and relative order
	/// This still provides their order, but doesn't reference them directly.
	/// The MixTextures have been moved to the Resources to support this
	/// </summary>
	[CreateAssetMenu(fileName = "MixTextureOrdering", menuName = "Scriptable Objects/Character Compositor/MixTextureOrdering")]
	public class MixTextureOrdering : ScriptableObject
	{
		[SerializeField] MixTextureOrderGroup[] _orderGroups;

		IEnumerable<MixTexture> _orderedMixTextures;

		public IEnumerable<MixTexture> OrderedMixTextures
		{
			get
			{
				if (_orderedMixTextures == null)
				{
					List<MixTexture> _ordered = new();
					var allMixTextures = ResourceLoader.LoadAllNoCache<MixTexture>();
					foreach (var group in _orderGroups)
					{
						var groupMixTextures = allMixTextures
							.Where(t => t.Order.Group == group)
							.OrderBy(t => t.Order.Index);
						_ordered.AddRange(groupMixTextures);
					}
					_orderedMixTextures = _ordered;
				}
				return _orderedMixTextures;
			}
		}
	}
	[System.Serializable]
	public class MixTextureOrderGroup_Old
	{
		[SerializeField] string _name;
		[SerializeField] MixTexture[] _mixTextures;

		public IEnumerable<MixTexture> MixTextures => _mixTextures;
	}
}

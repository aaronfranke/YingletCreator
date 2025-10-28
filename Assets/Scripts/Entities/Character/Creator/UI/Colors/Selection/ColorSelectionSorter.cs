using Character.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Character.Creator.UI
{
	public interface IColorSelectionSorter
	{
		IEnumerable<ReColorId> Sort(IEnumerable<ReColorId> ids);
		void PositionSorted(Transform root, GameObject newObject);
	}

	public class ColorSelectionSorter : MonoBehaviour, IColorSelectionSorter
	{
		const int NoIdInsertionPointOffset = 10000;

		private IMixTextureOrderer _mixTextureOrderer;

		[SerializeField] AssetReferenceT<ReColorId> _noIdInsertionPointReference;
		Dictionary<ReColorId, int> _valueLookup;

		void Awake()
		{
			_mixTextureOrderer = Singletons.GetSingleton<IMixTextureOrderer>();
			_valueLookup = new();
			int value = 1;
			foreach (var mixTexture in _mixTextureOrderer.GetOrderedMixTextures())
			{
				// Use the mix texture ordering to determine recolor ID ordering (a separate ordering would be a headache)
				var id = mixTexture.ReColorId;
				if (id == _noIdInsertionPointReference.LoadSync())
				{
					value += NoIdInsertionPointOffset;
				}
				if (id == null) continue;
				if (!_valueLookup.ContainsKey(id))
				{
					_valueLookup[id] = value;
					value += 1;
				}
			}
		}

		public void PositionSorted(Transform root, GameObject newObject)
		{
			var newValue = GetSortValue(newObject);

			// Default to last position
			int insertIndex = root.childCount - 1;

			// Find correct sibling index
			for (int i = 0; i < root.childCount - 1; i++)
			{
				Transform sibling = root.GetChild(i);
				int siblingValue = GetSortValue(sibling.gameObject);

				if (newValue < siblingValue)
				{
					insertIndex = i;
					break;
				}
			}

			// Move the new object to the correct index
			newObject.transform.SetSiblingIndex(insertIndex);
		}

		int GetSortValue(GameObject gameObject)
		{
			var id = gameObject.GetComponent<IColorSelectionReference>().Id;
			return GetSortValue(id);
		}

		int GetSortValue(ReColorId id)
		{
			if (_valueLookup.TryGetValue(id, out int value))
			{
				return value;
			}
			else
			{
				// Not ordered. Probably an eye; throw it in the middle 
				return NoIdInsertionPointOffset;
			}
		}

		public IEnumerable<ReColorId> Sort(IEnumerable<ReColorId> ids)
		{
			return ids.OrderBy(id => GetSortValue(id));
		}
	}
}
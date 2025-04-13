using Character.Compositor;
using Character.Data;
using System.Collections.Generic;
using UnityEngine;


namespace Character.Creator.UI
{
	public interface IColorSelectionSorter
	{
		void PositionSorted(GameObject newObject);
	}

	public class ColorSelectionSorter : MonoBehaviour, IColorSelectionSorter
	{
		const int NoIdInsertionPointOffset = 10000;

		[SerializeField] MixTextureOrdering _mixTextureOrdering;
		[SerializeField] ReColorId _noIdInsertionPoint;

		Dictionary<ReColorId, int> _valueLookup;

		void Awake()
		{
			_valueLookup = new();
			int value = 1;
			foreach (var mixTexture in _mixTextureOrdering.OrderedMixTextures)
			{
				// Use the mix texture ordering to determine recolor ID ordering (a separate ordering would be a headache)
				var id = mixTexture.ReColorId;
				if (id == _noIdInsertionPoint)
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

		public void PositionSorted(GameObject newObject)
		{
			var newValue = GetSortValue(newObject);

			// Default to last position
			int insertIndex = this.transform.childCount - 1;

			// Find correct sibling index
			for (int i = 0; i < this.transform.childCount - 1; i++)
			{
				Transform sibling = this.transform.GetChild(i);
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
			var reference = gameObject.GetComponent<IColorSelectionReference>().Id;
			if (_valueLookup.TryGetValue(reference, out int value))
			{
				return value;
			}
			else
			{
				// Not ordered. Probably an eye; just throw ita t 
				return NoIdInsertionPointOffset;
			}
		}
	}
}
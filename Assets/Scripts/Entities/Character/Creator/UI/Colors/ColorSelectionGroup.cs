using Character.Compositor;
using Character.Data;
using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator.UI
{
	public class ColorSelectionGroup : ReactiveBehaviour
	{
		ITextureGatherer _gatherer;
		[SerializeField] GameObject _colorSelectionPrefab;

		EnumerableDictReflector<ReColorId, GameObject> _enumerableReflector;

		private void Awake()
		{
			_gatherer = this.GetCharacterCreatorComponent<ITextureGatherer>();
			_enumerableReflector = new(Create, Delete);
			// Clean up any dummy objects under this
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		private GameObject Create(ReColorId id)
		{
			var go = Instantiate(_colorSelectionPrefab, this.transform);
			go.GetComponent<IColorSelectionReference>().Id = id;
			PositionSorted(go);
			return go;
		}

		private void Delete(GameObject gameObject)
		{
			Destroy(gameObject);
		}

		private void Start()
		{
			AddReflector(ReflectColors);
		}

		void ReflectColors()
		{
			var recolorIds = _gatherer.AllRelevantTextures
				.ToArray()
				.Select(t => t.ReColorId)
				.Where(i => i != null)
				.ToHashSet();
			_enumerableReflector.Enumerate(recolorIds);
		}


		void PositionSorted(GameObject newObject)
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
			// TODO: implement this properly
			var reference = gameObject.GetComponent<IColorSelectionReference>();
			return reference.Id.name.Length;
		}
	}
}
using Character.Compositor;
using Character.Data;
using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator.UI
{
	public class ColorSelectionGroup : ReactiveBehaviour
	{
		private IColorSelectionSorter _sorter;
		private ITextureGatherer _gatherer;
		[SerializeField] GameObject _colorSelectionPrefab;

		EnumerableDictReflector<ReColorId, GameObject> _enumerableReflector;

		private void Awake()
		{
			_sorter = this.GetComponent<IColorSelectionSorter>();
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
			_sorter.PositionSorted(go);
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

	}
}
using Character.Compositor;
using Character.Data;
using Reactivity;
using System.Linq;
using UnityEngine;

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
		return Instantiate(_colorSelectionPrefab, this.transform);
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

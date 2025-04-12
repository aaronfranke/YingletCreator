using Reactivity;
using UnityEngine;

public class ColorSelectionGroup : ReactiveBehaviour
{
	[SerializeField] GameObject _colorSelectionPrefab;

	private void Awake()
	{
		// Clean up any dummy objects under this
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void Start()
	{
		AddReflector(ReflectColors);
	}

	void ReflectColors()
	{
		// Debug
		for (int i = 0; i < 3; i++)
		{
			Instantiate(_colorSelectionPrefab, this.transform);
		}
	}
}

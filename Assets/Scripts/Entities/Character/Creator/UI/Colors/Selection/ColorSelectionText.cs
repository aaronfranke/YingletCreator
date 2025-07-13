using Reactivity;
using TMPro;
using UnityEngine;


namespace Character.Creator.UI
{
	public class ColorSelectionText : ReactiveBehaviour
	{
		[SerializeField] Material _selectedMaterial;
		private IColorSelectionReference _reference;
		private TMP_Text _text;
		private Material _startMaterial;

		private void Start()
		{
			_reference = this.GetComponentInParent<IColorSelectionReference>();
			_text = this.GetComponent<TMP_Text>();
			_text.text = _reference.Id.DisplayName;
			_startMaterial = _text.fontMaterial;
			AddReflector(ReflectTextMaterial);
		}

		private void ReflectTextMaterial()
		{
			_text.fontMaterial = _reference.Selected.Val ? _selectedMaterial : _startMaterial;
		}
	}
}
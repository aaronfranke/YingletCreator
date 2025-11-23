using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Character.Creator.UI
{
	public interface IColorSelectionClicking
	{
		event Action<bool> OnChange;
	}

	public class ColorSelectionClicking : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IColorSelectionClicking
	{
		private IColorActiveSelection _activeSelection;
		private IColorSelectionReference _reference;

		public event Action<bool> OnChange = delegate { };

		static bool _clickedDownOnAnyOfThese = false;

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
			_reference = this.GetComponent<IColorSelectionReference>();
		}
		private void OnDisable()
		{
			_clickedDownOnAnyOfThese = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Handle();
			StartCoroutine(KeepStaticUntilMouseUp());
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!Input.GetMouseButton(0)) return; // Need to be holding LMB
			if (_clickedDownOnAnyOfThese == false) return; // Need to have clicked down on one of these first
			Handle();
		}

		void Handle()
		{
			bool union = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			var result = _activeSelection.ToggleSelection(_reference.Id, union);
			OnChange(result);
		}

		IEnumerator KeepStaticUntilMouseUp()
		{
			_clickedDownOnAnyOfThese = true;
			while (Input.GetMouseButton(0))
			{
				yield return null;
			}
			_clickedDownOnAnyOfThese = false;
		}

	}
}
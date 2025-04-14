using System;
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

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
			_reference = this.GetComponent<IColorSelectionReference>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Handle();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!Input.GetMouseButton(0)) return;
			Handle();
		}

		void Handle()
		{
			bool union = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			var result = _activeSelection.ToggleSelection(_reference.Id, union);
			OnChange(result);
		}

	}
}
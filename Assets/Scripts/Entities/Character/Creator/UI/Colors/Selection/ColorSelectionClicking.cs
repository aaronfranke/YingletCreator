using UnityEngine;
using UnityEngine.EventSystems;


namespace Character.Creator.UI
{
	public class ColorSelectionClicking : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
	{
		private IColorActiveSelection _activeSelection;
		private IColorSelectionReference _reference;
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
			_activeSelection.ToggleSelection(_reference.Id, union);
		}

	}
}
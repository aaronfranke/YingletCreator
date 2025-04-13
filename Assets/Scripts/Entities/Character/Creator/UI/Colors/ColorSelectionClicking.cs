using UnityEngine;
using UnityEngine.EventSystems;


namespace Character.Creator.UI
{
	public class ColorSelectionClicking : MonoBehaviour, IPointerClickHandler
	{
		private IColorActiveSelection _activeSelection;
		private IColorSelectionReference _reference;
		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
			_reference = this.GetComponent<IColorSelectionReference>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			bool union = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			_activeSelection.ToggleSelection(_reference.Id, union);
		}
	}
}
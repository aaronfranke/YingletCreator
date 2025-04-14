using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class ScrollRectWithoutClickDrag : ScrollRect
	{
		public override void OnBeginDrag(PointerEventData eventData) { }
		public override void OnDrag(PointerEventData eventData) { }
		public override void OnEndDrag(PointerEventData eventData) { }
	}
}
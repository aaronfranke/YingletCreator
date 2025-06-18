using UnityEngine;
using UnityEngine.EventSystems;

public class PosePupilPositions : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
{
	private RectTransform _rectTransform;

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}
	public void OnDrag(PointerEventData eventData)
	{
		eventData.Use();
		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			_rectTransform,
			Input.mousePosition,
			null,
			out localPoint
		);

		Rect rect = _rectTransform.rect;

		Vector2 normalized = new Vector2(
			Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x),
			Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y)
		);

		Debug.Log($"Normalized Position: {normalized}");
	}

	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		eventData.useDragThreshold = false;
	}
}

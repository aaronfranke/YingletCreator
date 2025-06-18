using UnityEngine;
using UnityEngine.EventSystems;

public class PosePupilPositions : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
{
	private RectTransform _rectTransform;
	private IPosePupilUiData _pupilData;

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_pupilData = GetComponent<IPosePupilUiData>();
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
		_pupilData.PupilPosition = new Vector2(.5f, .5f) - normalized;
	}

	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		eventData.useDragThreshold = false;
	}
}

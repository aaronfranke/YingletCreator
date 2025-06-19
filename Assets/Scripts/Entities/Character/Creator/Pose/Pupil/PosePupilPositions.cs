using UnityEngine;
using UnityEngine.EventSystems;

public class PosePupilPositions : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler, IPointerClickHandler
{
	private RectTransform _rectTransform;
	private IPosePupilUiData _pupilData;
	private IDragSfx _dragSfx;

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_pupilData = GetComponent<IPosePupilUiData>();
		_dragSfx = GetComponent<IDragSfx>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		eventData.Use();
		ApplyPupilPosition(Input.mousePosition);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		eventData.Use();
		ApplyPupilPosition(eventData.position);
	}

	private void ApplyPupilPosition(Vector2 screenPosition)
	{
		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			_rectTransform,
			screenPosition,
			null,
			out localPoint
		);

		Rect rect = _rectTransform.rect;

		Vector2 normalized = new Vector2(
			Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x),
			Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y)
		);
		var from = _pupilData.PupilPosition;
		var to = new Vector2(.5f, .5f) - normalized;
		_pupilData.PupilPosition = to;
		_dragSfx.Change(Vector2.Distance(from, to));
	}

	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		eventData.useDragThreshold = false;
	}
}

using Reactivity;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiHoverable : MonoBehaviour, IHoverable, IPointerEnterHandler, IPointerExitHandler
{
	Observable<bool> _hovered = new(false);
	private IWriteableUiHoverManager _uiHoverManager;

	public IReadOnlyObservable<bool> Hovered => _hovered;

	void Awake()
	{
		_uiHoverManager = Singletons.GetSingleton<IWriteableUiHoverManager>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_hovered.Val = true;
		_uiHoverManager.SetCurrentHoverable(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_hovered.Val = false;
		_uiHoverManager.RemoveCurrentHoverable(this);
	}

	void OnDisable()
	{
		_hovered.Val = false;
		_uiHoverManager.RemoveCurrentHoverable(this);
	}
}

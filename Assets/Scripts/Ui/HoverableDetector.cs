using Reactivity;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverableDetector
{
    IReadOnlyObservable<bool> Hovered { get; }
}

public class HoverableDetector : MonoBehaviour, IHoverableDetector, IPointerEnterHandler, IPointerExitHandler
{
    Observable<bool> _hovered = new(false);

    public IReadOnlyObservable<bool> Hovered => _hovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovered.Val = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovered.Val = false;
    }
}

using Reactivity;
using UnityEngine.EventSystems;

public interface IBookmarkSelfSelection
{
    IReadOnlyObservable<bool> IsSelected { get; }
}

public class BookmarkSelfSelection : ReactiveBehaviour, IBookmarkSelfSelection, IPointerClickHandler
{
    private IClipboardSelection _clipboardSelection;

    Computed<bool> _isSelected;
    public IReadOnlyObservable<bool> IsSelected => _isSelected;

    void Awake()
    {
        _clipboardSelection = this.GetComponentInParent<IClipboardSelection>();
        _isSelected = CreateComputed<bool>(ComputeIsSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clipboardSelection.SetSelection(this);
    }

    private bool ComputeIsSelected()
    {
        return _clipboardSelection.Selection.Val == (IBookmarkSelfSelection)this;
    }
}

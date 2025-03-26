using Reactivity;
using UnityEngine;

/// <summary>
/// Creates and controls the "VisualOnly" version of this bookmark
/// This is used to fall off with the page during transitions
/// </summary>
public class BookmarkSelfVisualController : ReactiveBehaviour
{
    [SerializeField] GameObject _visualOnlyPrefab;

    private BookmarkImageControl _imageControl;
    private IBookmarkPageControl _pageControl;
    private IBookmarkSelfSelection _selfSelection;
    private IClipboardOrdering _clipboardOrdering;

    private BookmarkImageControl _visualOnlyImageControl;

    private void Awake()
    {
        _imageControl = this.GetComponent<BookmarkImageControl>();
        _pageControl = this.GetComponent<IBookmarkPageControl>();
        _selfSelection = this.GetComponent<IBookmarkSelfSelection>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
        CreateVisualOnlyBookmark();
    }

    private void Start()
    {
        this.AddReflector(ReflectSelected);
    }

    private void ReflectSelected()
    {
        bool isSelected = _selfSelection.IsSelected.Val;
        _pageControl.Page.SetActive(isSelected);
        if (isSelected)
        {
            _clipboardOrdering.SendToFront(_pageControl.Page.transform);
            _clipboardOrdering.SendToFront(this.transform);
        }

    }

    void CreateVisualOnlyBookmark()
    {
        var go = Instantiate(_visualOnlyPrefab, this.transform.parent);
        _visualOnlyImageControl = go.GetComponent<BookmarkImageControl>();
        _imageControl.CopyValuesTo(_visualOnlyImageControl);
        go.SetActive(false);
        _clipboardOrdering.SendToBack(go.transform);
    }
}
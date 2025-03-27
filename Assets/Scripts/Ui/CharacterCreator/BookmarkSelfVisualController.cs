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
    private IBookmarkSelfSelection _selfSelection;
    private IClipboardOrdering _clipboardOrdering;


    private void Awake()
    {
        _imageControl = this.GetComponent<BookmarkImageControl>();
        _selfSelection = this.GetComponent<IBookmarkSelfSelection>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
        CreateFakeBookmark();
    }

    private void Start()
    {
        this.AddReflector(ReflectSelected);
    }

    private void ReflectSelected()
    {
        bool isSelected = _selfSelection.IsSelected.Val;
        if (isSelected)
        {
            _clipboardOrdering.SendToFront(this.transform, isFreeFall: false);
        }
    }

    void CreateFakeBookmark()
    {
        var fakeBookmark = Instantiate(_visualOnlyPrefab, this.transform.parent).GetComponent<IFakeBookmark>();
        fakeBookmark.Setup(this.gameObject);
    }
}
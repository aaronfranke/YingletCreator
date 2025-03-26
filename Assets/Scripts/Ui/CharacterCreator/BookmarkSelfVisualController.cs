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
    private BookmarkImageControl _visualOnlyImageControl;

    private void Awake()
    {
        _imageControl = this.GetComponent<BookmarkImageControl>();
        _pageControl = this.GetComponent<IBookmarkPageControl>();
        _selfSelection = this.GetComponent<IBookmarkSelfSelection>();
        CreateVisualOnlyBookmark();
    }

    private void Start()
    {
        this.AddReflector(ReflectSelected);
    }

    private void ReflectSelected()
    {
        _pageControl.Page.SetActive(_selfSelection.IsSelected.Val);
    }

    void CreateVisualOnlyBookmark()
    {
        _visualOnlyImageControl = Instantiate(_visualOnlyPrefab, this.transform.parent).GetComponent<BookmarkImageControl>();
        _imageControl.CopyValuesTo(_visualOnlyImageControl);
        _visualOnlyImageControl.gameObject.SetActive(false);
    }
}
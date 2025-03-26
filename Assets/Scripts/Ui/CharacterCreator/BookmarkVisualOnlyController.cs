using UnityEngine;

/// <summary>
/// Creates and controls the "VisualOnly" version of this bookmark
/// This is used to fall off with the page during transitions
/// </summary>
public class BookmarkVisualOnlyController : MonoBehaviour
{
    [SerializeField] GameObject _visualOnlyPrefab;
    private BookmarkImageControl _imageControl;
    private BookmarkImageControl _visualOnlyImageControl;

    private void Awake()
    {
        _imageControl = this.GetComponent<BookmarkImageControl>();
        CreateVisualOnlyBookmark();
    }

    void CreateVisualOnlyBookmark()
    {
        _visualOnlyImageControl = Instantiate(_visualOnlyPrefab, this.transform.parent).GetComponent<BookmarkImageControl>();
        _imageControl.CopyValuesTo(_visualOnlyImageControl);
        _visualOnlyImageControl.gameObject.SetActive(false);
    }
}
using Reactivity;
using UnityEngine;

/// <summary>
/// Creates and controls the "VisualOnly" version of this bookmark
/// This is used to fall off with the page during transitions
/// </summary>
public class BookmarkSelfVisualController : ReactiveBehaviour
{
    [SerializeField] GameObject _visualOnlyPrefab;

    private IClipboardElementSelection _elementSelection;
    private IClipboardOrdering _clipboardOrdering;


    private void Awake()
    {
        _elementSelection = this.GetComponent<IClipboardElementSelection>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
    }

    private void Start()
    {
        CreateFakeBookmark();
        this.AddReflector(ReflectSelected);
    }

    private void ReflectSelected()
    {
        bool isSelected = _elementSelection.IsSelected.Val;
        _clipboardOrdering.SendToLayer(this.transform, isSelected ? ClipboardLayer.ActiveBookmark : ClipboardLayer.Back);
    }

    void CreateFakeBookmark()
    {
        var fakeBookmark = Instantiate(_visualOnlyPrefab, this.transform.parent).GetComponent<IFakeBookmark>();
        fakeBookmark.Setup(this.gameObject);
    }
}
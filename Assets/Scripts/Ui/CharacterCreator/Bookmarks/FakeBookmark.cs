using Reactivity;
using System.Collections;
using UnityEngine;

/// <summary>
/// When the user clicks off the page, this will move with the page
/// </summary>
public interface IFakeBookmark
{
    void Setup(GameObject realBookmark);
}

public class FakeBookmark : MonoBehaviour, IFakeBookmark
{
    [SerializeField] RectTransform _animMotionRoot;
    private Animation _animation;
    private BookmarkImageControl _imageControl;
    private RealBookmarkReference _realReference;
    private IClipboardOrdering _clipboardOrdering;
    private Coroutine _freeFallCoroutine;

    void Awake()
    {
        _animation = this.GetComponent<Animation>();
        _imageControl = this.GetComponent<BookmarkImageControl>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
    }

    void Start()
    {
        _realReference.IsRealSelected.OnChanged += Selected_OnChanged;
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _realReference.IsRealSelected.OnChanged -= Selected_OnChanged;
    }

    public void Setup(GameObject realBookmark)
    {
        _realReference = new RealBookmarkReference(realBookmark);
        _realReference.ImageControl.CopyValuesTo(_imageControl);
        this.transform.position = _realReference.Transform.position;
        _clipboardOrdering.SendToBack(this.transform);
    }

    private void Selected_OnChanged(bool wasSelected, bool isSelected)
    {
        if (!wasSelected) return;

        _animation.Stop();
        _clipboardOrdering.SendToFront(this.transform, isFreeFall: true);
        this.gameObject.SetActive(true);
        this.StopAndStartCoroutine(ref _freeFallCoroutine, FreeFall());
    }

    IEnumerator FreeFall()
    {
        var page = _realReference.PageReference.Page;
        _animMotionRoot.localPosition = Vector3.zero;
        _animMotionRoot.localRotation = Quaternion.identity;
        page.SetParent(_animMotionRoot);
        _animation.Play();
        yield return new WaitForSeconds(_animation.clip.length);
        _animation.Stop();


        // Still our parent? Disable this
        page.DisableIfStillParented(_animMotionRoot);
    }
}

sealed class RealBookmarkReference
{
    public RectTransform Transform { get; }
    public IReadOnlyObservable<bool> IsRealSelected { get; }
    public BookmarkImageControl ImageControl { get; }
    public IBookmarkPageReference PageReference { get; }

    public RealBookmarkReference(GameObject realBookmark)
    {
        ImageControl = realBookmark.GetComponent<BookmarkImageControl>();
        PageReference = realBookmark.GetComponent<IBookmarkPageReference>();
        IsRealSelected = realBookmark.GetComponent<IBookmarkSelfSelection>().IsSelected;
        Transform = realBookmark.GetComponent<RectTransform>();
    }
}
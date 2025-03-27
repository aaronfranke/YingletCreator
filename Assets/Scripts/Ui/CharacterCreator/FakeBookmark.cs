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
    [SerializeField] float _freeFallTime = 10f;

    private BookmarkImageControl _imageControl;
    private RealBookmarkReference _realReference;
    private IClipboardOrdering _clipboardOrdering;
    private Coroutine _freeFallCoroutine;

    void Awake()
    {
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

    private void Selected_OnChanged(bool _, bool isSelected)
    {
        if (!isSelected) return;

        _clipboardOrdering.SendToFront(this.transform);
        this.gameObject.SetActive(true);
        this.StopAndStartCoroutine(ref _freeFallCoroutine, FreeFall());
    }

    IEnumerator FreeFall()
    {
        this.transform.position = _realReference.Transform.position;
        this.transform.rotation = _realReference.Transform.rotation;
        float startTime = Time.time;
        while (Time.time < startTime + _freeFallTime)
        {
            this.transform.position += Vector3.down * 100 * Time.deltaTime;
            yield return null;
        }
    }
}

sealed class RealBookmarkReference
{
    public RectTransform Transform { get; }
    public IReadOnlyObservable<bool> IsRealSelected { get; }
    public BookmarkImageControl ImageControl { get; }
    public GameObject Page { get; }
    public RealBookmarkReference(GameObject realBookmark)
    {
        ImageControl = realBookmark.GetComponent<BookmarkImageControl>();
        Page = realBookmark.GetComponent<IBookmarkPageControl>().Page;
        IsRealSelected = realBookmark.GetComponent<IBookmarkSelfSelection>().IsSelected;
        Transform = realBookmark.GetComponent<RectTransform>();
    }
}

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
    [SerializeField] FakeBookmarkFreefallSettings _freeFallSettings;

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

    private void Selected_OnChanged(bool wasSelected, bool isSelected)
    {
        if (!wasSelected) return;

        _clipboardOrdering.SendToFront(this.transform, isFreeFall: true);
        this.gameObject.SetActive(true);
        this.StopAndStartCoroutine(ref _freeFallCoroutine, FreeFall());
    }

    IEnumerator FreeFall()
    {
        var page = _realReference.PageReference.Page.transform;

        this.transform.position = _realReference.Transform.position;
        this.transform.rotation = _realReference.Transform.rotation;
        page.SetParent(this.transform, true);
        Vector3 velocity =
            Vector3.right * Random.Range(_freeFallSettings.HorizontalSpeedRange.x, _freeFallSettings.HorizontalSpeedRange.y) +
            Vector3.up * Random.Range(_freeFallSettings.VerticalSpeedRange.x, _freeFallSettings.VerticalSpeedRange.y);

        float startTime = Time.time;
        while (Time.time < startTime + _freeFallSettings.Duration)
        {
            this.transform.position += velocity * Time.deltaTime;
            velocity += Vector3.up * _freeFallSettings.Gravity * Time.deltaTime;
            yield return null;
        }

        // Still our parent? Disable this
        if (page.transform.parent == this.transform)
        {
            _realReference.PageReference.Page.SetActive(false);
        }
    }
}

sealed class RealBookmarkReference
{
    public RectTransform Transform { get; }
    public IReadOnlyObservable<bool> IsRealSelected { get; }
    public BookmarkImageControl ImageControl { get; }
    public IBookmarkPageControl PageReference { get; }

    public RealBookmarkReference(GameObject realBookmark)
    {
        ImageControl = realBookmark.GetComponent<BookmarkImageControl>();
        PageReference = realBookmark.GetComponent<IBookmarkPageControl>();
        IsRealSelected = realBookmark.GetComponent<IBookmarkSelfSelection>().IsSelected;
        Transform = realBookmark.GetComponent<RectTransform>();
    }
}

[System.Serializable]
sealed class FakeBookmarkFreefallSettings
{
    public float Duration = 10f;
    public Vector2 HorizontalSpeedRange;
    public Vector2 VerticalSpeedRange;
    public float Gravity;
}
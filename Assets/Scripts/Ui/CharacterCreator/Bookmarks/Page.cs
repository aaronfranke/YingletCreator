using Reactivity;
using UnityEngine;
using UnityEngine.UI;

public interface IPage
{
    void Connect(IBookmarkSelfSelection bookmark);

    void SetParent(Transform newParent);
    void DisableIfStillParented(Transform compareParent);

}
public class Page : ReactiveBehaviour, IPage
{
    [SerializeField] Image _tintImage;
    [SerializeField] Color _startTintColor;
    [SerializeField] EaseSettings _untintEaseSettings;

    private Transform _originalParent;
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private CanvasGroup _canvasGroup;
    private IClipboardOrdering _clipboardOrdering;
    private IBookmarkSelfSelection _bookmark;
    private Coroutine _tintCoroutine;

    void Awake()
    {
        _originalParent = this.transform.parent;
        _originalPos = this.transform.localPosition;
        _originalRot = this.transform.localRotation;
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
        _tintImage.color = Color.clear;
        _tintImage.gameObject.SetActive(false);
    }

    public void Connect(IBookmarkSelfSelection bookmark)
    {
        _bookmark = bookmark;
        AddReflector(ReflectSelected);
    }


    private void ReflectSelected()
    {
        bool isSelected = _bookmark.IsSelected.Val;
        _canvasGroup.interactable = isSelected;
        if (isSelected)
        {
            this.gameObject.SetActive(true);
            ResetTransform();
            _clipboardOrdering.SendToFront(this.transform, isFreeFall: false);

            _tintImage.gameObject.SetActive(true);
            this.StartEaseCoroutine(ref _tintCoroutine, _untintEaseSettings,
                p => _tintImage.color = Color.Lerp(_startTintColor, Color.clear, p),
                () => _tintImage.gameObject.SetActive(false));
        }
    }

    void ResetTransform()
    {
        this.transform.SetParent(_originalParent, true);
        this.transform.localPosition = _originalPos;
        this.transform.localRotation = _originalRot;
    }

    public void SetParent(Transform newParent)
    {
        this.transform.SetParent(newParent, true);
    }

    public void DisableIfStillParented(Transform compareParent)
    {
        if (this.transform.parent == compareParent)
        {
            this.gameObject.SetActive(false);
        }
    }
}

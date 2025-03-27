using Reactivity;
using UnityEngine;

public interface IPage
{
    void Connect(IBookmarkSelfSelection bookmark);

    void SetParent(Transform newParent);
    void DisableIfStillParented(Transform compareParent);

}
public class Page : ReactiveBehaviour, IPage
{
    private Transform _originalParent;
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private CanvasGroup _canvasGroup;
    private IClipboardOrdering _clipboardOrdering;
    private IBookmarkSelfSelection _bookmark;

    void Awake()
    {
        _originalParent = this.transform.parent;
        _originalPos = this.transform.localPosition;
        _originalRot = this.transform.localRotation;
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _clipboardOrdering = this.GetComponentInParent<IClipboardOrdering>();
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

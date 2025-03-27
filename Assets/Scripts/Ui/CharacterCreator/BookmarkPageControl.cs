using UnityEngine;

public interface IBookmarkPageControl
{
    GameObject Page { get; }
    void ResetTransform();
    void MakeInteractable(bool val);
}

public class BookmarkPageControl : MonoBehaviour, IBookmarkPageControl
{
    [SerializeField] GameObject _page;

    private Transform _originalParent;
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private CanvasGroup _canvasGroup;

    public GameObject Page => _page;


    void Awake()
    {
        _originalParent = _page.transform.parent;
        _originalPos = _page.transform.localPosition;
        _originalRot = _page.transform.localRotation;
        _canvasGroup = _page.GetComponent<CanvasGroup>();
    }

    public void MakeInteractable(bool val)
    {
        _canvasGroup.interactable = val;
    }

    public void ResetTransform()
    {
        _page.transform.SetParent(_originalParent, true);
        _page.transform.localPosition = _originalPos;
        _page.transform.localRotation = _originalRot;
    }

}

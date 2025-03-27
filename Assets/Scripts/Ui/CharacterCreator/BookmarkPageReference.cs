using UnityEngine;

public interface IBookmarkPageReference
{
    IPage Page { get; }
}

public class BookmarkPageReference : MonoBehaviour, IBookmarkPageReference
{
    [SerializeField] Page _page;

    public IPage Page => _page;

    void Awake()
    {
        _page.Connect(this.GetComponent<IBookmarkSelfSelection>());
    }
}

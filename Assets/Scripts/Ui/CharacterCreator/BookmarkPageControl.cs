using UnityEngine;

public interface IBookmarkPageControl
{
    GameObject Page { get; }
}

public class BookmarkPageControl : MonoBehaviour, IBookmarkPageControl
{
    [SerializeField] GameObject _page;
    public GameObject Page => _page;
}

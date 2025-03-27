using UnityEngine;
using UnityEngine.UI;

public interface IBookmarkImageControl
{
    void CopyValuesFrom(IBookmarkImageControl source);
}

public class BookmarkImageControl : MonoBehaviour, IBookmarkImageControl
{
    [SerializeField] Image _fill;
    [SerializeField] Image _icon;
    [SerializeField] Color _selectedIconColor = Color.white;
    private Color _unselectedColor;

    private void Awake()
    {
        _unselectedColor = _fill.color;
    }

    public void CopyValuesFrom(IBookmarkImageControl source)
    {
        var sourceImpl = (BookmarkImageControl)source;
        _unselectedColor = sourceImpl._unselectedColor;
        _fill.color = sourceImpl._fill.color;
        _icon.color = sourceImpl._icon.color;
        _icon.sprite = sourceImpl._icon.sprite;
    }
}

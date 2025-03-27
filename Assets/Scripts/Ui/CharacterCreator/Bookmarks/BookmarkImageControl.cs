using UnityEngine;
using UnityEngine.UI;

public class BookmarkImageControl : MonoBehaviour
{
    [SerializeField] Image _fill;
    [SerializeField] Image _icon;
    [SerializeField] Color _selectedIconColor = Color.white;
    private Color _unselectedColor;

    private void Awake()
    {
        _unselectedColor = _fill.color;
    }

    public void CopyValuesTo(BookmarkImageControl destination)
    {
        destination._unselectedColor = _unselectedColor;
        destination._fill.color = _fill.color;
        destination._icon.color = _icon.color;
        destination._icon.sprite = _icon.sprite;
    }
}

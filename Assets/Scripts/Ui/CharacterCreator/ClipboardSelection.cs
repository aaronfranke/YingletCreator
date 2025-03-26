using Reactivity;
using UnityEngine;


public interface IClipboardSelection
{
    Observable<IBookmarkSelfSelection> Selection { get; }

    void SetSelection(IBookmarkSelfSelection selection);
}

public class ClipboardSelection : MonoBehaviour, IClipboardSelection
{
    [SerializeField] BookmarkSelfSelection _initialSelection;

    public Observable<IBookmarkSelfSelection> Selection { get; } = new Observable<IBookmarkSelfSelection>();

    void Awake()
    {
        Selection.Val = _initialSelection;
    }

    public void SetSelection(IBookmarkSelfSelection selection)
    {
        Selection.Val = selection;
    }
}

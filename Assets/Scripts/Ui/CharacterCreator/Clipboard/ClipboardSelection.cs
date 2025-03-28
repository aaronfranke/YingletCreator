using Reactivity;
using System.Linq;
using UnityEngine;

public enum ClipboardSelectionType
{
    Overview = 0,
    Palette = 1,
    Sliders = 2
}

public interface IClipboardSelection
{
    Observable<ClipboardSelectionType> Selection { get; }

    void SetSelection(ClipboardSelectionType selection);
    IPage GetPageWithType(ClipboardSelectionType type);
}

public class ClipboardSelection : MonoBehaviour, IClipboardSelection
{
    [SerializeField] ClipboardSelectionType _initialSelection;

    public Observable<ClipboardSelectionType> Selection { get; } = new Observable<ClipboardSelectionType>();

    void Start()
    {
        Selection.Val = _initialSelection;
    }

    public void SetSelection(ClipboardSelectionType selection)
    {
        Selection.Val = selection;
    }

    public IPage GetPageWithType(ClipboardSelectionType type)
    {
        return this.GetComponentsInChildren<IPage>(true).First(page => page.GetComponent<IClipboardElementSelection>().Type == type);
    }
}

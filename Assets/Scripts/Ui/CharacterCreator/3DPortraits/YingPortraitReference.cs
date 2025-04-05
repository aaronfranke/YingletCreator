using Character.Creator;
using Reactivity;
using TMPro;

public interface IYingPortraitReference
{
    void Setup(CachedYingletReference reference);
    public CachedYingletReference Reference { get; }
    bool IsSelected { get; }
}

public class YingPortraitReference : ReactiveBehaviour, IYingPortraitReference
{
    private ICustomizationSelection _selection;
    private Computed<bool> _selected;
    private Observable<CachedYingletReference> _reference = new();

    public CachedYingletReference Reference => _reference.Val;
    public bool IsSelected => _selected.Val;

    void Awake()
    {
        _selection = this.GetComponentInParent<ICustomizationSelection>();
        _selected = CreateComputed(ComputeSelected);
    }

    public void Setup(CachedYingletReference reference)
    {
        _reference.Val = reference;
        GetComponentInChildren<TextMeshProUGUI>().text = reference.CachedData.Name;
    }

    bool ComputeSelected()
    {
        return _reference.Val == _selection.Selected;
    }
}

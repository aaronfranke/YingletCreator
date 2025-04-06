using Reactivity;

namespace Character.Creator.UI
{
    public interface IYingPortraitReference : ISelectable
    {
        void Setup(CachedYingletReference reference);
        public CachedYingletReference Reference { get; }
    }

    public class YingPortraitReference : ReactiveBehaviour, IYingPortraitReference
    {
        private ICustomizationSelection _selection;
        private Computed<bool> _selected;
        private Observable<CachedYingletReference> _reference = new();

        public CachedYingletReference Reference => _reference.Val;
        public IReadOnlyObservable<bool> Selected => _selected;

        void Awake()
        {
            _selection = this.GetComponentInParent<ICustomizationSelection>();
            _selected = CreateComputed(ComputeSelected);
        }

        public void Setup(CachedYingletReference reference)
        {
            _reference.Val = reference;
        }

        bool ComputeSelected()
        {
            return _reference.Val == _selection.Selected;
        }
    }
}
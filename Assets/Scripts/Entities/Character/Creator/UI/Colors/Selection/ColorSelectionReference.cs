using Character.Data;
using Reactivity;


namespace Character.Creator.UI
{
	public interface IColorSelectionReference : ISelectable
	{
		public ReColorId Id { get; set; }
	}

	public class ColorSelectionReference : ReactiveBehaviour, IColorSelectionReference
	{
		private IColorActiveSelection _activeSelection;
		private Computed<bool> _computeSelected;

		public ReColorId Id { get; set; }

		public IReadOnlyObservable<bool> Selected => _computeSelected;

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
		}

		void Start()
		{
			_computeSelected = this.CreateComputed(ComputeSelected);
		}

		private bool ComputeSelected()
		{
			return _activeSelection.CheckSelected(Id);
		}
	}
}
using Reactivity;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggleSelection : ReactiveBehaviour, ISelectable
	{
		Computed<bool> _selected;
		private ICustomizationSelectedDataRepository _dataRepo;
		private ICharacterCreatorToggleReference _reference;

		public IReadOnlyObservable<bool> Selected => _selected;

		void Awake()
		{
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
			_reference = this.GetComponent<ICharacterCreatorToggleReference>();
		}

		void Start()
		{
			_selected = CreateComputed(ComputeSelected);
		}

		private bool ComputeSelected()
		{
			var toggleVal = _dataRepo.GetToggle(_reference.ToggleId);
			return toggleVal;
		}
	}
}
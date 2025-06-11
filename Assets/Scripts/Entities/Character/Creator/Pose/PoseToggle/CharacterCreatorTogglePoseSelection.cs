using Reactivity;

namespace Character.Creator.UI
{
	public class CharacterCreatorTogglePoseSelection : ReactiveBehaviour, ISelectable
	{
		Computed<bool> _selected;
		private IPageYingPoseData _dataRepo;
		private ICharacterCreatorTogglePoseReference _reference;

		public IReadOnlyObservable<bool> Selected => _selected;

		void Awake()
		{
			_dataRepo = this.GetComponentInParent<IPageYingPoseData>();
			_reference = this.GetComponent<ICharacterCreatorTogglePoseReference>();
		}

		void Start()
		{
			_selected = CreateComputed(ComputeSelected);
		}

		private bool ComputeSelected()
		{
			if (_dataRepo.Data == null) return false;

			var toggleVal = _dataRepo.Data.Pose == _reference.PoseId;
			return toggleVal;
		}
	}
}
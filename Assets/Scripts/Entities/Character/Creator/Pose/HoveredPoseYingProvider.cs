using Reactivity;

public interface IHoveredPoseYingProvider
{
	PoseYing HoveredPoseYing { get; }
}

public class HoveredPoseYingProvider : ReactiveBehaviour, IHoveredPoseYingProvider
{
	Computed<PoseYing> _activelyHoveredPoseYing;
	private IColliderHoverManager _hoverManager;

	public PoseYing HoveredPoseYing => _activelyHoveredPoseYing.Val;

	void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_activelyHoveredPoseYing = this.CreateComputed(Compute);
	}

	private PoseYing Compute()
	{
		var currentlyHovered = _hoverManager.CurrentlyHovered;
		var poseDataRepo = currentlyHovered?.gameObject?.GetComponent<IPoseYingDataRepository>();
		if (poseDataRepo == null) return null;
		return new PoseYing(poseDataRepo.Reference, currentlyHovered.gameObject);
	}
}



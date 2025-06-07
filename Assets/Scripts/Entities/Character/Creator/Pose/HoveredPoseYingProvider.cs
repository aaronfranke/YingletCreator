using Character.Creator;
using Reactivity;
using UnityEngine;

public interface IHoveredPoseYingProvider
{
	HoveredPoseYing HoveredPoseYing { get; }
}

public class HoveredPoseYingProvider : ReactiveBehaviour, IHoveredPoseYingProvider
{
	Computed<HoveredPoseYing> _activelyHoveredPoseYing;
	private IColliderHoverManager _hoverManager;

	public HoveredPoseYing HoveredPoseYing => _activelyHoveredPoseYing.Val;

	void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_activelyHoveredPoseYing = this.CreateComputed(Compute);
	}

	private HoveredPoseYing Compute()
	{
		var currentlyHovered = _hoverManager.CurrentlyHovered;
		var poseDataRepo = currentlyHovered?.gameObject?.GetComponent<IPoseYingDataRepository>();
		if (poseDataRepo == null) return null;
		return new HoveredPoseYing(poseDataRepo.Reference, currentlyHovered.gameObject);
	}
}

public sealed class HoveredPoseYing
{
	public CachedYingletReference Reference { get; }
	public GameObject GameObject { get; }
	public HoveredPoseYing(CachedYingletReference reference, GameObject gameObject)
	{
		Reference = reference;
		GameObject = gameObject;
	}
}

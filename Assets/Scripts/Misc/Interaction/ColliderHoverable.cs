using Reactivity;
using UnityEngine;

public interface IColliderHoverable : IHoverable
{
	/// <summary>
	/// Greater values will cause this to be selected over lower values
	/// </summary>
	int PriorityFudge { get; }
}

public class ColliderHoverable : ReactiveBehaviour, IColliderHoverable
{
	[SerializeField] int _priorityFudge = 0;

	Computed<bool> _hovered;
	private IColliderHoverManager _hoverManager;

	public IReadOnlyObservable<bool> Hovered => _hovered;
	public int PriorityFudge => _priorityFudge;

	void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_hovered = this.CreateComputed(ComputeHovered);
	}

	private bool ComputeHovered()
	{
		return _hoverManager.CurrentlyHovered == (IColliderHoverable)this;
	}
}

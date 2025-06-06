using Reactivity;

public class ColliderHoverable : ReactiveBehaviour, IHoverable
{
	Computed<bool> _hovered;
	private IColliderHoverManager _hoverManager;

	public IReadOnlyObservable<bool> Hovered => _hovered;

	void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		_hovered = this.CreateComputed(ComputeHovered);
	}

	private bool ComputeHovered()
	{
		return _hoverManager.CurrentlyHovered == (IHoverable)this;
	}
}

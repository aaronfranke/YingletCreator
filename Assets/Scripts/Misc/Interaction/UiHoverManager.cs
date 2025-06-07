using Reactivity;

public interface IUiHoverManager
{
	bool HoveringUi { get; }
	void RegisterNonUiHoverable(IHoverable hoverable);
}

public class UiHoverManager : ReactiveBehaviour, IUiHoverManager
{
	private IHoverable _nonUiHoverable;
	Computed<bool> _hoveringUi;

	void Awake()
	{
		_hoveringUi = CreateComputed(ComputeHoveringUi);
	}

	private bool ComputeHoveringUi()
	{
		return !_nonUiHoverable.Hovered.Val;
	}

	public bool HoveringUi => _hoveringUi.Val;

	public void RegisterNonUiHoverable(IHoverable hoverable)
	{
		_nonUiHoverable = hoverable;
	}
}

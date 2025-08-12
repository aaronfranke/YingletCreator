using Reactivity;

public interface IUiHoverManager
{
	bool HoveringUi { get; }
	IHoverable Current { get; }
}

public interface IWriteableUiHoverManager : IUiHoverManager
{
	void SetCurrentHoverable(IHoverable hoverable);

	/// <summary>
	/// Pass in the hoverable to verify this is even still what we consider being hovered,
	/// or sometimes Hoverable sets this if is disabled
	/// </summary>
	void RemoveCurrentHoverable(IHoverable hoverable);

	void RegisterNonUiHoverable(IHoverable hoverable);
}

public class UiHoverManager : ReactiveBehaviour, IWriteableUiHoverManager
{
	private IHoverable _nonUiHoverable;
	Observable<IHoverable> _current = new(null);
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

	public IHoverable Current => _current.Val;


	public void SetCurrentHoverable(IHoverable hoverable)
	{
		_current.Val = hoverable;
	}

	public void RemoveCurrentHoverable(IHoverable hoverable)
	{
		if (_current.Val != hoverable) return;
		_current.Val = null;
	}
	public void RegisterNonUiHoverable(IHoverable hoverable)
	{
		_nonUiHoverable = hoverable;
	}
}

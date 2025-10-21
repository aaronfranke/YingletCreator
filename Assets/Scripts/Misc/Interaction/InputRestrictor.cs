using Reactivity;

/// <summary>
/// Returns if normal, keyboard input and mouse controls should be allowed
/// Restrictions will occur if the user is viewing a confirmation, settings, or about screen
/// </summary>
public interface IInputRestrictor
{
	bool InputAllowed { get; }
}
public class InputRestrictor : ReactiveBehaviour, IInputRestrictor
{
	private IMenuManager _menuManager;
	private IConfirmationManager _confirmationManager;
	private Computed<bool> _inputAllowed;

	public bool InputAllowed => _inputAllowed.Val;

	private void Awake()
	{
		_menuManager = Singletons.GetSingleton<IMenuManager>();
		_confirmationManager = Singletons.GetSingleton<IConfirmationManager>();
		_inputAllowed = CreateComputed(ComputeInputAllowed);
	}

	private bool ComputeInputAllowed()
	{
		if (_menuManager.OpenMenu.Val != null) return false;
		if (_confirmationManager.Current.Val != null) return false;

		return true;
	}
}

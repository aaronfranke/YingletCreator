using Reactivity;
using TMPro;

public class ReflectMenuButtonText : ReactiveBehaviour
{
	private IUiHoverManager _uiHoverManager;
	private Computed<IMenuButton> _menuButton;
	private TMP_Text _text;

	void Start()
	{
		_uiHoverManager = Singletons.GetSingleton<IUiHoverManager>();
		_menuButton = CreateComputed(ComputeHoveredMenuButton);
		_text = this.GetComponent<TMP_Text>();
		AddReflector(ReflectText);
	}

	private IMenuButton ComputeHoveredMenuButton()
	{
		return _uiHoverManager.Current?.gameObject?.GetComponent<IMenuButton>();
	}

	private void ReflectText()
	{
		_text.text = _menuButton.Val?.Type switch
		{
			MenuButtonType.Exit => "Exit",
			MenuButtonType.Settings => "Settings",
			MenuButtonType.About => "About",
			MenuButtonType.Discord => "Discord",
			_ => ""
		};
	}
}

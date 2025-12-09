using Reactivity;
using TMPro;

public class TooltipPresenter : ReactiveBehaviour, ISelectable
{
	private TMP_Text _text;
	private ITooltipManager _tooltipManager;

	Computed<bool> _selected;
	public IReadOnlyObservable<bool> Selected => _selected;

	private void Awake()
	{
		_text = this.GetComponentInChildren<TMPro.TMP_Text>();
		_tooltipManager = Singletons.GetSingleton<ITooltipManager>();
		_selected = CreateComputed(ComputeSelected);
		AddReflector(Reflect);
	}

	private bool ComputeSelected()
	{
		return _tooltipManager.CurrentTooltip.Val != null;
	}

	void Reflect()
	{
		var currentTooltip = _tooltipManager.CurrentTooltip.Val;
		if (currentTooltip == null) return;
		_text.text = currentTooltip.Text;
	}
}

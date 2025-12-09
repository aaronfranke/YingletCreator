using Reactivity;
using UnityEngine;

public interface ITooltipManager
{
	void Register(ITooltip tooltip);
	void Unregister(ITooltip tooltip);
	IReadOnlyObservable<ITooltip> CurrentTooltip { get; }
}
public class TooltipManager : MonoBehaviour, ITooltipManager
{
	Observable<ITooltip> _currentTooltip = new Observable<ITooltip>(null);

	public IReadOnlyObservable<ITooltip> CurrentTooltip => _currentTooltip;

	public void Register(ITooltip tooltip)
	{
		_currentTooltip.Val = tooltip;
	}

	public void Unregister(ITooltip tooltip)
	{
		if (_currentTooltip.Val == tooltip)
		{
			_currentTooltip.Val = null;
		}
	}
}

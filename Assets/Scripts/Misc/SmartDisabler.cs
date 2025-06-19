using Reactivity;
using System.Linq;

/// <summary>
/// If multiple components want to disable a game-object, it can be challenging to manage
/// This allows multiple components to disable a game-object by keeping track of all of the disablers separately
/// </summary>
public interface ISmartDisabler
{
	void SetActive(bool active, object reference);
}

public class SmartDisabler : ReactiveBehaviour, ISmartDisabler
{
	ObservableHashSet<object> _disablers = new();
	Computed<bool> _active;

	private void Awake()
	{
		_active = this.CreateComputed(ComputeActive);
		AddReflector(ReflectActive);
	}

	private bool ComputeActive()
	{
		return !_disablers.Any();
	}

	private void ReflectActive()
	{
		this.gameObject.SetActive(_active.Val);
	}

	public void SetActive(bool active, object reference)
	{
		if (active)
		{
			_disablers.Remove(reference);
		}
		else
		{
			_disablers.Add(reference);
		}
	}
}

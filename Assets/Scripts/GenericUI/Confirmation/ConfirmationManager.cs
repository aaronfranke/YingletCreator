using Reactivity;
using System;
using UnityEngine;

public sealed class ConfirmationData
{
	public ConfirmationData(string displayText, string confirmText, string id, Action confirmAction)
	{
		DisplayText = displayText;
		ConfirmText = confirmText;
		Id = id;
		ConfirmAction = confirmAction;
	}

	public string DisplayText { get; }
	public string ConfirmText { get; }
	public string Id { get; }
	public Action ConfirmAction { get; }
}

public interface IConfirmationManager
{
	IReadOnlyObservable<ConfirmationData> Current { get; }

	void OpenConfirmation(ConfirmationData data);

	void Execute();
	void Cancel();
}

public class ConfirmationManager : MonoBehaviour, IConfirmationManager
{
	Observable<ConfirmationData> _current = new Observable<ConfirmationData>(null);
	public IReadOnlyObservable<ConfirmationData> Current => _current;

	public void Cancel()
	{
		_current.Val = null;
	}

	public void Execute()
	{
		_current.Val.ConfirmAction();
		_current.Val = null;
	}

	public void OpenConfirmation(ConfirmationData data)
	{
		_current.Val = data;
	}
}

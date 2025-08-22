using System;
using UnityEngine;

/// <summary>
/// This class doesn't really _do_ anything beyond forward events
/// The expectation is that some 
/// </summary>
public interface IToastMediator
{
	void ShowToast(string message);
	event Action<string> OnToastRequested;
}

public class ToastMediator : MonoBehaviour, IToastMediator
{
	public event Action<string> OnToastRequested = delegate { };

	public void ShowToast(string message)
	{
		OnToastRequested?.Invoke(message);

		Debug.Log($"Toast requested: {message}", this);
	}
}

using UnityEngine;

/// <summary>
/// This class doesn't really _do_ anything beyond forward events
/// The expectation is that some 
/// </summary>
public interface IToastDisplay
{
	void Show(string message);
}

public class ToastDisplay : MonoBehaviour, IToastDisplay
{
	[SerializeField] GameObject _toastPrefab;

	public void Show(string message)
	{
		var go = GameObject.Instantiate(_toastPrefab, this.transform);

		go.GetComponentInChildren<TMPro.TMP_Text>().text = message;
	}
}

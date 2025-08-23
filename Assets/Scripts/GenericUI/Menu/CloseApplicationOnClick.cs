using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloseApplicationOnClick : MonoBehaviour
{
	private Button _button;
	private IScreenTransitioner _screenTransitioner;
	bool _quitting = false;

	void Awake()
	{
		_button = this.GetComponent<Button>();
		_screenTransitioner = Singletons.GetSingleton<IScreenTransitioner>();
		_button.onClick.AddListener(Button_OnClick);
	}

	private void OnDestroy()
	{
		_button.onClick.RemoveListener(Button_OnClick);
	}

	private void Button_OnClick()
	{
		if (_quitting) return;
		StartCoroutine(TransitionThenQuit());
	}

	private IEnumerator TransitionThenQuit()
	{
		_quitting = true;

		_screenTransitioner.TransitionToOpaque();
		yield return new WaitForSeconds(_screenTransitioner.TransitionTime);

		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}

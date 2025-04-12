using TMPro;
using UnityEngine;

public class PlaySoundOnTextboxLetter : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;
	private IAudioPlayer _audioPlayer;
	private TMP_InputField _inputField;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_inputField = this.GetComponent<TMP_InputField>();
		_inputField.onValueChanged.AddListener(InputField_OnValueChanged);
	}

	private void OnDestroy()
	{
		_inputField.onValueChanged.RemoveListener(InputField_OnValueChanged);
	}

	private void InputField_OnValueChanged(string arg0)
	{
		if (Time.timeSinceLevelLoad < .5f) return;
		_audioPlayer.Play(_soundEffect);
	}
}

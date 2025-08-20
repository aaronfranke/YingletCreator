using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnToggleClicked : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;
	private IAudioPlayer _audioPlayer;
	private Toggle _toggle;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_toggle = this.GetComponent<Toggle>();
		_toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
	}

	private void OnDestroy()
	{
		_toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
	}

	private void Toggle_OnValueChanged(bool isOn)
	{
		if (!isOn) return;
		_audioPlayer.Play(_soundEffect);
	}
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PlaySoundsOnToggleUI : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundOn;
	[SerializeField] private SoundEffect _soundOff;
	private IAudioPlayer _audioPlayer;
	private Toggle _toggle;

	void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_toggle = this.GetComponent<Toggle>();
		_toggle.onValueChanged.AddListener(PlayToggleSound);
	}

	void OnDestroy()
	{
		_toggle.onValueChanged.RemoveListener(PlayToggleSound);
	}

	void PlayToggleSound(bool isOn)
	{
		_audioPlayer.Play(isOn ? _soundOn : _soundOff);
	}
}

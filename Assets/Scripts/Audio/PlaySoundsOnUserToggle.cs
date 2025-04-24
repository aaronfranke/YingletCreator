using UnityEngine;

public class PlaySoundsOnUserToggle : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundOn;
	[SerializeField] private SoundEffect _soundOff;
	private IAudioPlayer _audioPlayer;
	private IUserToggleEvents _toggleEvents;

	void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_toggleEvents = this.GetComponent<IUserToggleEvents>();
		_toggleEvents.UserToggled += PlayToggleSound;
	}

	void OnDestroy()
	{
		_toggleEvents.UserToggled -= PlayToggleSound;
	}

	void PlayToggleSound(bool isOn)
	{
		_audioPlayer.Play(isOn ? _soundOn : _soundOff);
	}
}

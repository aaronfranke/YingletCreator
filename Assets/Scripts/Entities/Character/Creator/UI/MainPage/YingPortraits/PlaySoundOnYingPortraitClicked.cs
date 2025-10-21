using Character.Creator.UI;
using UnityEngine;

public class PlaySoundOnYingPortraitClicked : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;
	private IAudioPlayer _audioPlayer;
	private YingPortraitClicking _yingPortraitClicking;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_yingPortraitClicking = this.GetComponent<YingPortraitClicking>();
		_yingPortraitClicking.OnSelected += On;
	}

	private void OnDestroy()
	{
		_yingPortraitClicking.OnSelected -= On;
	}

	void On()
	{
		_audioPlayer.Play(_soundEffect);
	}
}

using Character.Creator.UI;
using UnityEngine;

public class PlaySoundOnYingDeletetion : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;
	private IAudioPlayer _audioPlayer;
	private DeleteOnButtonClick _deleteOnButtonClick;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_deleteOnButtonClick = this.GetComponent<DeleteOnButtonClick>();
		_deleteOnButtonClick.OnDelete += On;
	}

	private void OnDestroy()
	{
		_deleteOnButtonClick.OnDelete -= On;
	}

	void On()
	{
		_audioPlayer.Play(_soundEffect);
	}
}

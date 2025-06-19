using UnityEngine;

public class PlaySoundOnPicture : MonoBehaviour
{
	[SerializeField] private SoundEffect _pictureTaken;
	private IAudioPlayer _audioPlayer;
	private ITakePictureEvents _picEvents;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_picEvents = this.GetComponent<ITakePictureEvents>();
		_picEvents.PictureTaken += OnPictureTaken;
	}

	private void OnDestroy()
	{
		_picEvents.PictureTaken -= OnPictureTaken;
	}

	private void OnPictureTaken()
	{
		_audioPlayer.Play(_pictureTaken);
	}
}

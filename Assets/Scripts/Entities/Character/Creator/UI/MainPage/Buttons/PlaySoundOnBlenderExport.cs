using UnityEngine;

public class PlaySoundOnBlenderExport : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;
	private IAudioPlayer _audioPlayer;
	private ExportToBlenderOnButtonClick _deleteOnButtonClick;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_deleteOnButtonClick = this.GetComponent<ExportToBlenderOnButtonClick>();
		_deleteOnButtonClick.OnExport += On;
	}

	private void OnDestroy()
	{
		_deleteOnButtonClick.OnExport -= On;
	}

	void On()
	{
		_audioPlayer.Play(_soundEffect);
	}
}

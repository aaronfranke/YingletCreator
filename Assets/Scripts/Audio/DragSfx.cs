using System.Collections;
using UnityEngine;

public interface IDragSfx
{
	void Change(float delta);
}

public class DragSfx : MonoBehaviour, IDragSfx
{
	[SerializeField] private SoundEffect _sound;
	[SerializeField] float _volumeDecreaseSpeed;
	[SerializeField] float _sliderDeltaToVolume;
	[SerializeField] float _minimumVolumeToAdd;

	private IAudioPlayer _audioPlayer;
	private Coroutine _coroutine;
	float _volume = 0f;
	private AudioSource _currentAudioPlayer;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
	}

	public void Change(float delta)
	{
		_volume = Mathf.Max(_minimumVolumeToAdd, _volume);
		_volume = Mathf.Clamp01(_volume + delta * _sliderDeltaToVolume);

		this.StartCoroutineIfNotAlreadyRunning(ref _coroutine, Play());
	}

	private void OnDisable()
	{
		if (_coroutine != null) StopCoroutine(_coroutine);
		StopPlaying();
	}

	IEnumerator Play()
	{
		_currentAudioPlayer = _audioPlayer.Play(_sound, new AudioPlayOptions() { AutoDestroy = false });
		_currentAudioPlayer.loop = true;

		while (_volume > -1f)
		{
			_currentAudioPlayer.volume = Mathf.Clamp01(_volume) * _sound.Volume;
			_volume -= _volumeDecreaseSpeed * Time.deltaTime;
			yield return null;
		}
		StopPlaying();
	}

	void StopPlaying()
	{
		if (_currentAudioPlayer != null)
		{
			Destroy(_currentAudioPlayer.gameObject);
			_currentAudioPlayer = null;
		}
		_coroutine = null;
	}
}

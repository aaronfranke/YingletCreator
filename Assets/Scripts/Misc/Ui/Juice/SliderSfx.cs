using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSfx : MonoBehaviour
{
	[SerializeField] private SoundEffect _changePage;
	[SerializeField] float _volumeDecreaseSpeed;
	[SerializeField] float _sliderDeltaToVolume;
	[SerializeField] float _minimumVolumeToAdd;

	private IAudioPlayer _audioPlayer;
	private Slider _slider;
	private Coroutine _coroutine;
	private float _previousValue = 0.5f;
	float _volume = 0f;
	private AudioSource _currentAudioPlayer;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_slider = this.GetComponent<Slider>();

		_slider.onValueChanged.AddListener(Slider_OnValueChanged);
	}

	private void Slider_OnValueChanged(float value)
	{
		if (EventSystem.current.currentSelectedGameObject != _slider.gameObject)
		{
			return;
		}

		float delta = Mathf.Abs(value - _previousValue);
		_volume = Mathf.Max(_minimumVolumeToAdd, _volume);
		_volume = Mathf.Clamp01(_volume + delta * _sliderDeltaToVolume);

		this.StartCoroutineIfNotAlreadyRunning(ref _coroutine, Play());
		_previousValue = value;
	}

	private void OnDisable()
	{
		if (_coroutine != null) StopCoroutine(_coroutine);
		StopPlaying();
	}

	private void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
	}

	IEnumerator Play()
	{
		_currentAudioPlayer = _audioPlayer.Play(_changePage, new AudioPlayOptions() { AutoDestroy = false });
		_currentAudioPlayer.loop = true;

		while (_volume > -1f)
		{
			_currentAudioPlayer.volume = Mathf.Clamp01(_volume) * _changePage.Volume;
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

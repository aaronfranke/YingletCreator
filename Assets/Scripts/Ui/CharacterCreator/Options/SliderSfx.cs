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

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
    }

    IEnumerator Play()
    {
        var player = _audioPlayer.Play(_changePage, new AudioPlayOptions() { AutoDestroy = false });
        player.loop = true;

        while (_volume > -1f)
        {
            player.volume = Mathf.Clamp01(_volume) * _changePage.Volume;
            _volume -= _volumeDecreaseSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(player.gameObject);
        _coroutine = null;
    }
}

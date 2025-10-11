using UnityEngine;

public class PlaySoundOnBooped : MonoBehaviour
{

    [SerializeField] private SoundEffect _soundEffect;
    private IAudioPlayer _audioPlayer;
    private IBoopManager _boopManager;

    private void Awake()
    {
        _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
        _boopManager = this.GetComponentInParent<IBoopManager>();

        _boopManager.OnBoop += OnBooped;
    }

    private void OnDestroy()
    {
        _boopManager.OnBoop -= OnBooped;
    }

    void OnBooped()
    {
        _audioPlayer.Play(_soundEffect);
    }
}

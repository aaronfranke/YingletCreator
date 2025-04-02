using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSelect : MonoBehaviour, ISelectHandler
{
    [SerializeField] private SoundEffect _soundEffect;
    private IAudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _audioPlayer.Play(_soundEffect);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnButtonClicked : MonoBehaviour
{
    [SerializeField] private SoundEffect _soundEffect;
    private IAudioPlayer _audioPlayer;
    private Button _button;

    private void Awake()
    {
        _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
        _button = this.GetComponent<Button>();
        _button.onClick.AddListener(Button_OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Button_OnClick);
    }

    private void Button_OnClick()
    {
        _audioPlayer.Play(_soundEffect);
    }
}

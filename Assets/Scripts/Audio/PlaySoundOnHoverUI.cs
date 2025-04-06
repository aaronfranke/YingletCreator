using UnityEngine;

[RequireComponent(typeof(Hoverable))]
public class PlaySoundOnHoverUI : MonoBehaviour
{
    [SerializeField] private SoundEffect _soundEffect;
    private IAudioPlayer _audioPlayer;
    private IHoverable _hoverable;

    private void Awake()
    {
        _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
        _hoverable = this.GetComponent<IHoverable>();
    }

    private void Start()
    {
        _hoverable.Hovered.OnChanged += Hovered_OnChanged;
    }

    private void OnDestroy()
    {
        _hoverable.Hovered.OnChanged -= Hovered_OnChanged;
    }
    private void Hovered_OnChanged(bool from, bool to)
    {
        if (!to) return;
        _audioPlayer.Play(_soundEffect);
    }
}

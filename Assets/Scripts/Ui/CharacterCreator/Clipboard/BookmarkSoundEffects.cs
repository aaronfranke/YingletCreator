using UnityEngine;

public class BookmarkSoundEffects : MonoBehaviour
{
    [SerializeField] private SoundEffect _hoverBookmark;
    private IAudioPlayer _audioPlayer;
    private IHoverableDetector _hoverable;
    private IClipboardElementSelection _selection;

    private void Awake()
    {
        _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
        _hoverable = this.GetComponent<IHoverableDetector>();
        _selection = this.GetComponent<IClipboardElementSelection>();
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
        if (_selection.IsSelected.Val) return;
        _audioPlayer.Play(_hoverBookmark);
    }
}

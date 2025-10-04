using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameMusicDefinition
{
    public AudioClip Clip;
    public int Repeats = 1;
}

public class CharacterCreatorMusicPlayer : MonoBehaviour
{
    [SerializeField] private MenuType _titleScreenMenu;

    [SerializeField] AudioClip _introClip;

    [SerializeField] GameMusicDefinition[] _gameMusic;

    [SerializeField] float _titleStartDelay = .5f;
    [SerializeField] float _titleMusicFadeOutTime = 0.5f;
    [SerializeField] float _betweenMusicDelay = 1.5f;

    private IAudioMixerProvider _mixerProvider;
    private IMenuManager _menuManager;
    private AudioSource _source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _menuManager = Singletons.GetSingleton<IMenuManager>();
        _mixerProvider = Singletons.GetSingleton<IAudioMixerProvider>();

        _menuManager.OpenMenu.OnChanged += Menu_OnOpenChanged;

        _source = this.gameObject.AddComponent<AudioSource>();
        _source.clip = _introClip;
        _source.loop = true;
        _source.outputAudioMixerGroup = _mixerProvider.MusicGroup;
        _source.Stop();

        StartCoroutine(StartAfterDelay());
    }

    private void OnDestroy()
    {
        Destroy(_source);
    }

    private void Menu_OnOpenChanged(MenuType from, MenuType to)
    {
        if (from == _titleScreenMenu && to != _titleScreenMenu)
        {
            StartCoroutine(FadeOutIntoGameplay());
        }
    }

    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_titleStartDelay);

        _source.Play();
    }

    IEnumerator FadeOutIntoGameplay()
    {
        for (float t = 0; t < _titleMusicFadeOutTime; t += Time.deltaTime)
        {
            float p = t / _titleMusicFadeOutTime;
            _source.volume = (1 - p);
            yield return null;
        }
        _source.volume = 0;
        _source.Stop();

        yield return new WaitForSeconds(_betweenMusicDelay);

        _source.clip = _gameMusic[Random.Range(0, _gameMusic.Length)].Clip;
        _source.volume = 1;
        _source.Play();
    }
}

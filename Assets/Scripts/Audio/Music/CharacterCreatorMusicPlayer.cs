using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameMusicDefinition
{
    public AudioClip Clip;
    public int Repeats = 1;
    public bool FadeOut = false;
    public float Volume = 1;
}

public class CharacterCreatorMusicPlayer : MonoBehaviour
{
    [SerializeField] private MenuType _titleScreenMenu;

    [SerializeField] AudioClip _introClip;

    [SerializeField] GameMusicDefinition[] _gameMusic;

    [SerializeField] float _titleStartDelay = .5f;
    [SerializeField] float _titleMusicFadeOutTime = 0.5f;
    [SerializeField] float _postTitleDelay = .7f;
    [SerializeField] float _betweenMusicDelay = 1.5f;
    [SerializeField] float _gameMusicFadeOutTime = 1;

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

        yield return new WaitForSeconds(_postTitleDelay);
        StartCoroutine(PlayMusicPerpetually());
    }

    IEnumerator PlayMusicPerpetually()
    {
        int currentSong = 0;
        while (true)
        {
            yield return new WaitForSeconds(_betweenMusicDelay);

            var music = _gameMusic[currentSong];

            _source.clip = music.Clip;
            _source.volume = music.Volume;
            _source.Play();

            if (music.Repeats > 1)
            {
                _source.loop = true;
            }
            else
            {
                _source.loop = false;
            }

            StartCoroutine(WaitAndRemoveLoop(music));
            StartCoroutine(WaitAndFadeOut(music));
            yield return new WaitForSeconds(music.Clip.length * music.Repeats);

            currentSong = (currentSong + 1) % _gameMusic.Length;
        }
    }

    IEnumerator WaitAndRemoveLoop(GameMusicDefinition music)
    {
        if (_source.loop == false) yield break; // No need to run this logic, we're already not looping

        yield return new WaitForSeconds(music.Clip.length * music.Repeats - _gameMusicFadeOutTime);
        _source.loop = false;
    }

    IEnumerator WaitAndFadeOut(GameMusicDefinition music)
    {
        if (music.FadeOut == false) yield break; // No need to run this logic, we're not fading out
        yield return new WaitForSeconds(music.Clip.length * music.Repeats - _gameMusicFadeOutTime);
        var fromVolume = _source.volume;
        for (float t = 0; t < _gameMusicFadeOutTime; t += Time.deltaTime)
        {
            float p = t / _gameMusicFadeOutTime;
            _source.volume = Mathf.Lerp(fromVolume, 0, p);
            yield return null;
        }
        _source.volume = 0;
    }
}

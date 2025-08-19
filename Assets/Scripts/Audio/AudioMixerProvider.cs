using UnityEngine;
using UnityEngine.Audio;

public interface IAudioMixerProvider
{
	AudioMixer Mixer { get; }
	AudioMixerGroup SoundEffectsGroup { get; }
	AudioMixerGroup MusicGroup { get; }
}

public class AudioMixerProvider : MonoBehaviour, IAudioMixerProvider
{
	[SerializeField] AudioMixer _mixer;
	[SerializeField] AudioMixerGroup _soundEffectsGroup;
	[SerializeField] AudioMixerGroup _musicGroup;

	public AudioMixer Mixer => _mixer;
	public AudioMixerGroup SoundEffectsGroup => _soundEffectsGroup;
	public AudioMixerGroup MusicGroup => _musicGroup;
}

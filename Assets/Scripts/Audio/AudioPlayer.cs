using UnityEngine;

public interface IAudioPlayer
{
    void Play(ISoundEffect soundEffect);
}

public class AudioPlayer : MonoBehaviour, IAudioPlayer
{
    public void Play(ISoundEffect soundEffect)
    {
        var go = new GameObject(soundEffect.Name);
        var source = go.AddComponent<AudioSource>();
        source.clip = soundEffect.Clip;
        source.loop = false;
        source.volume = Mathf.Min(1, soundEffect.Volume); // Might eventually need to multiply this by some input for dynamic sound effects
        //source.outputAudioMixerGroup = _audioMixerHandler.SfxGroup;
        source.Play();

        GameObject.Destroy(go, soundEffect.Clip.length + .25f);
    }
}

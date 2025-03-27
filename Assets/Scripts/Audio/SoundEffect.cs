using UnityEngine;

/// <summary>
/// Wrapper around AudioClip with some extra configurations
/// </summary>
public interface ISoundEffect
{
    string Name { get; }
    AudioClip Clip { get; }
    float Volume { get; }
}

[CreateAssetMenu(fileName = "SoundEffect", menuName = "Scriptable Objects/SoundEffect")]
public class SoundEffect : ScriptableObject, ISoundEffect
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _volume = 1;

    public string Name => name;
    public AudioClip Clip => _clip;
    public float Volume => _volume;
}

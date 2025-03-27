using UnityEngine;

/// <summary>
/// Wrapper around AudioClip with some extra configurations
/// </summary>
public interface ISoundEffect
{
    string Name { get; }
    AudioClip Clip { get; }
    float Volume { get; }
    Vector2 RandomPitchRange { get; }
}

[CreateAssetMenu(fileName = "SoundEffect", menuName = "Scriptable Objects/SoundEffect")]
public class SoundEffect : ScriptableObject, ISoundEffect
{
    [SerializeField] AudioClip _clip;
    [SerializeField][Range(0, 1)] float _volume = 1;
    [SerializeField][Tooltip("Values between 0 and 3, with 1 being no pitch shift")] Vector2 _randomPitchRange = Vector2.one;

    public string Name => name;
    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public Vector2 RandomPitchRange => _randomPitchRange;
}

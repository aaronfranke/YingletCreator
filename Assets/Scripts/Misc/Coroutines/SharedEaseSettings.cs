using UnityEngine;

/// <summary>
/// Unlike the serializable <see cref="EaseSettings"/>, these can be generated to a scriptable object and shared easily
/// </summary>
[CreateAssetMenu(fileName = "Ease", menuName = "Scriptable Objects/EaseSettings")]
public class SharedEaseSettings : ScriptableObject, IEaseSettings
{
    [SerializeField] AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] float _duration = 1;
    public AnimationCurve Curve => _curve;
    public float Duration => _duration;
}

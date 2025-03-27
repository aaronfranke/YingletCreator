using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public sealed class EaseSettings
{
    [SerializeField] AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] float _duration = 1;

    public AnimationCurve Curve => _curve;
    public float Duration => _duration;
}

public static class CoroutineUtils
{
    public static void StopAndStartCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine existingCoroutine, IEnumerator routine)
    {

        if (existingCoroutine != null)
        {
            monoBehaviour.StopCoroutine(existingCoroutine);
        }
        existingCoroutine = monoBehaviour.StartCoroutine(routine);
    }

    public static void StartEaseCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine existingCoroutine, EaseSettings settings, Action<float> apply, Action onComplete = null)
    {
        StopAndStartCoroutine(monoBehaviour, ref existingCoroutine, Ease(settings, apply, onComplete));
    }

    static IEnumerator Ease(EaseSettings settings, Action<float> apply, Action onComplete)
    {
        for (float t = 0; t < settings.Duration; t += Time.deltaTime)
        {
            float p = t / settings.Duration;
            apply(settings.Curve.Evaluate(p));
            yield return null;
        }
        apply(settings.Curve.Evaluate(1));
        if (onComplete != null) onComplete();
    }
}

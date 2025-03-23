using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IBlinkTimer
{
    event Action OnBlink;
}

public class BlinkTimer : MonoBehaviour, IBlinkTimer
{
    [SerializeField] Vector2 _blinkTimeRange; // Humans blink between 2 and 4

    public event Action OnBlink = delegate { };

    void Start()
    {
        StartCoroutine(RepeatedBlinks());
    }

    IEnumerator RepeatedBlinks()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_blinkTimeRange.x, _blinkTimeRange.y));
            OnBlink();
        }
    }
}

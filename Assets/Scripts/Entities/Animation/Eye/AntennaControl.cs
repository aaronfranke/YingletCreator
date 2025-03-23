using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


sealed class Antenna
{
    private readonly Transform _transform;
    private readonly Quaternion _originalRotation;

    public Antenna(Transform transform)
    {
        _transform = transform;
        _originalRotation = transform.localRotation;
    }

    public void SetRotation(float angle)
    {
        _transform.localRotation = _originalRotation * Quaternion.Euler(angle, 0, 0);
    }
}

public class AntennaControl : MonoBehaviour
{
    [SerializeField] EaseSettings _blinkEaseSettings;
    [SerializeField] Vector2 _blinkAngleRotations;

    private IBlinkTimer _blinkTimer;
    private IEnumerable<Antenna> _antennas;
    Coroutine _blinkCoroutine;

    void Awake()
    {
        _blinkTimer = this.GetComponent<IBlinkTimer>();

        var antennaTransforms = TransformUtils.FindChildrenByPrefix(this.transform, "antenna_");
        _antennas = antennaTransforms.Select(a => new Antenna(a)).ToArray();
        if (!_antennas.Any()) Debug.LogWarning("Did not find any antennas to control");

        SetAntennaRotations(_blinkAngleRotations.y);

        _blinkTimer.OnBlink += BlinkTimer_OnBlink;
    }

    private void OnDestroy()
    {
        _blinkTimer.OnBlink -= BlinkTimer_OnBlink;
    }


 
    private void BlinkTimer_OnBlink()
    {
        CoroutineUtils.StartEaseCoroutine(this, ref _blinkCoroutine, _blinkEaseSettings, p =>
        {
            float angle = Mathf.Lerp(_blinkAngleRotations.x, _blinkAngleRotations.y, p);
            SetAntennaRotations(angle);
        });
    }

    void SetAntennaRotations(float angle)
    {
        foreach (var antenna in _antennas)
        {
            antenna.SetRotation(angle);
        }
    }
}

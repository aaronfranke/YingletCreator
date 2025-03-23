using System;
using System.Collections;
using UnityEngine;

public enum EyeExpression
{
    Normal,
    Squint,
    Closed
}

[RequireComponent(typeof(IEyeGatherer))]
public class EyeExpressions : MonoBehaviour
{
    [SerializeField] float _squintTime = .015f;
    [SerializeField] float _closedTime = .03f;

    private IEyeGatherer _eyeGatherer;
    private IBlinkTimer _blinkTimer;
    static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");

    void Awake()
    {
        _eyeGatherer = this.GetComponent<IEyeGatherer>();
        _blinkTimer = this.GetComponent<IBlinkTimer>();

        _blinkTimer.OnBlink += BlinkTimer_OnBlink;
    }

    void OnDestroy()
    {
        _blinkTimer.OnBlink -= BlinkTimer_OnBlink;
    }

    private void BlinkTimer_OnBlink()
    {
        StartCoroutine(Blink());
    }
    IEnumerator Blink()
    {
        SetEyesToExpression(EyeExpression.Squint);
        yield return new WaitForSeconds(_squintTime);
        SetEyesToExpression(EyeExpression.Closed);
        yield return new WaitForSeconds(_closedTime);
        SetEyesToExpression(EyeExpression.Squint);
        yield return new WaitForSeconds(_squintTime);
        SetEyesToExpression(EyeExpression.Normal);
    }

    void SetEyesToExpression(EyeExpression eyeExpression)
    {
        foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
        {
            eyeMaterial.SetInteger(EXPRESSION_PROPERTY_ID, (int)eyeExpression);
        }
    }
}

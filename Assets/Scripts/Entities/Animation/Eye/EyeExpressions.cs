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
    [SerializeField] Vector2 _blinkTimeRange; // Humans blink between 2 and 4 seconds
    [SerializeField] float _squintTime = .015f;
    [SerializeField] float _closedTime = .03f;

    private IEyeGatherer _eyeGatherer;

    static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");

    void Awake()
    {
        _eyeGatherer = this.GetComponent<IEyeGatherer>();
    }

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {

            yield return new WaitForSeconds(Random.Range(_blinkTimeRange.x, _blinkTimeRange.y));
            SetEyesToExpression(EyeExpression.Squint);
            yield return new WaitForSeconds(_squintTime);
            SetEyesToExpression(EyeExpression.Closed);
            yield return new WaitForSeconds(_closedTime);
            SetEyesToExpression(EyeExpression.Squint);
            yield return new WaitForSeconds(_squintTime);
            SetEyesToExpression(EyeExpression.Normal);
        }
    }

    void SetEyesToExpression(EyeExpression eyeExpression)
    {
        foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
        {
            eyeMaterial.SetInteger(EXPRESSION_PROPERTY_ID, (int)eyeExpression);
        }
    }
}

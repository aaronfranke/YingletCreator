using System.Collections;
using UnityEngine;

public class EyeWobble : MonoBehaviour, IEyeOffsetProvider
{
    [SerializeField] float _maxWobbleX;
    [SerializeField] float _maxWobbleY;
    [SerializeField] Vector2 _wobbleTimeRange;
    [SerializeField] EaseSettings _wobbleEaseSettings;

    Vector2 _wobbleAmount = Vector2.zero;

    public Vector2 Offset => _wobbleAmount;

    Coroutine _moveEye;

    void Start()
    {
        StartCoroutine(Wobble());
    }

    IEnumerator Wobble()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_wobbleTimeRange.x, _wobbleTimeRange.y));
            var targetWobbleAmount = new Vector2(Random.Range(-_maxWobbleX, _maxWobbleX), Random.Range(-_maxWobbleY, _maxWobbleY));
            this.StartEaseCoroutine(ref _moveEye, _wobbleEaseSettings, p => _wobbleAmount = Vector2.Lerp(_wobbleAmount, targetWobbleAmount, p));
        }
    }
}

using Reactivity;
using System.Collections;
using UnityEngine;

public class MouthExpressionsMutator_Booped : MonoBehaviour, IMouthExpressionsMutator
{
    [SerializeField] float WaitTime = 2f;

    private IBoopManager _boopManager;
    Observable<bool> _isBooped = new();
    private Coroutine _boopCoroutine;

    void Awake()
    {
        _boopManager = this.GetComponentInParent<IBoopManager>();
        _boopManager.OnBoop += OnBooped;
    }

    void OnDestroy()
    {
        _boopManager.OnBoop -= OnBooped;
    }
    private void OnEnable()
    {
        _isBooped.Val = false;
    }

    private void OnBooped()
    {
        if (!this.isActiveAndEnabled) return;
        this.StopAndStartCoroutine(ref _boopCoroutine, Booped());
    }

    IEnumerator Booped()
    {
        _isBooped.Val = true;
        yield return new WaitForSeconds(WaitTime);
        _isBooped.Val = false;
    }
    public void Mutate(ref MouthExpression expression, ref MouthOpenAmount openAmount)
    {
        if (!_isBooped.Val) return;

        expression = MouthExpression.Muse;
        openAmount = MouthOpenAmount.Closed;
    }
}

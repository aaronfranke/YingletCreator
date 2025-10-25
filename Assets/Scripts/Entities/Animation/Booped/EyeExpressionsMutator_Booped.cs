using Reactivity;
using System.Collections;
using UnityEngine;

[System.Serializable]
class EyeExpressionWithTime
{
	public EyeExpression State = EyeExpression.Normal;
	public float Time = 1;
}

public class EyeExpressionsMutator_Booped : MonoBehaviour, ICurrentEyeExpressionMutator
{
	[SerializeField] EyeExpressionWithTime[] _boopedStates;

	private IBoopManager _boopManager;
	Observable<bool> _isBooped = new();
	Observable<EyeExpression> _expression = new Observable<EyeExpression>();
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
		foreach (var state in _boopedStates)
		{
			_expression.Val = state.State;
			yield return new WaitForSeconds(state.Time);
		}
		_isBooped.Val = false;
	}

	public EyeExpression Mutate(EyeExpression input)
	{
		if (!_isBooped.Val) return input;
		return _expression.Val;
	}
}

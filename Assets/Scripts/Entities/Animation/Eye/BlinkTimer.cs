using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IBlinkTimer
{
	event Action OnBlink;
}

public class BlinkTimer : MonoBehaviour, IBlinkTimer
{
	[SerializeField] Vector2 _blinkTimeRange; // Humans blink between 2 and 4

	private IEyeExpressions _eyeExpressions;
	public event Action OnBlink = delegate { };

	static EyeExpression[] IGNORE_EXPRESSIONS = new EyeExpression[]
	{
		EyeExpression.Closed,
		EyeExpression.ClosedHappy,
		EyeExpression.ClosedEnergy,
	};

	void Start()
	{
		_eyeExpressions = this.GetComponent<IEyeExpressions>();
		StartCoroutine(RepeatedBlinks());
	}

	IEnumerator RepeatedBlinks()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_blinkTimeRange.x, _blinkTimeRange.y));

			// Don't blink antenna if the eye is already closed
			if (!IGNORE_EXPRESSIONS.Contains(_eyeExpressions.BaseExpression))
			{
				OnBlink();
			}
		}
	}
}

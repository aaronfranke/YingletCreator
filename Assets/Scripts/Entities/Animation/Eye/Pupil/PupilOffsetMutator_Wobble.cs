using Reactivity;
using System.Collections;
using UnityEngine;

public class PupilOffsetMutator_Wobble : ReactiveBehaviour, IPupilOffsetMutator
{
	[SerializeField] float _maxWobbleX;
	[SerializeField] float _maxWobbleY;
	[SerializeField] Vector2 _wobbleTimeRange;
	[SerializeField] EaseSettings _wobbleEaseSettings;

	Observable<Vector2> _wobbleAmount = new(Vector2.zero);

	Coroutine _moveEye;


	public PupilOffsets Mutate(PupilOffsets input)
	{
		return input.ShiftBothBy(_wobbleAmount.Val);
	}

	void OnEnable()
	{
		// Must be OnEnable instead of Start since the coroutine stops when this gets disabled and re-enabled
		StartCoroutine(Wobble());
	}

	IEnumerator Wobble()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_wobbleTimeRange.x, _wobbleTimeRange.y));
			var targetWobbleAmount = new Vector2(Random.Range(-_maxWobbleX, _maxWobbleX), Random.Range(-_maxWobbleY, _maxWobbleY));
			// Note: _wobbleAmount probably should have been cached here for use in the function, but w/e
			this.StartEaseCoroutine(ref _moveEye, _wobbleEaseSettings, p => _wobbleAmount.Val = Vector2.Lerp(_wobbleAmount.Val, targetWobbleAmount, p));
		}
	}
}

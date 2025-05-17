using Reactivity;
using UnityEngine;

public enum MouthExpression
{
	Grin,
	// Frown
	// O
}

public enum MouthOpenAmount
{
	Closed,
	Ajar,
	Open,
	PLACEHOLDER, // The fourth slot is currently empty
	MAX
}

public interface IMouthExpressions
{
	MouthExpression Expression { get; }
	MouthOpenAmount OpenAmount { get; }
}

public class MouthExpressions : MonoBehaviour, IMouthExpressions
{
	Observable<MouthExpression> _expression = new();
	Observable<MouthOpenAmount> _openAmount = new(MouthOpenAmount.Closed);

	public MouthExpression Expression => _expression.Val;
	public MouthOpenAmount OpenAmount => _openAmount.Val;

	public void EditorOpen()
	{
		_openAmount.Val = (MouthOpenAmount)((int)(_openAmount.Val + 1) % (int)MouthOpenAmount.PLACEHOLDER);
	}
}

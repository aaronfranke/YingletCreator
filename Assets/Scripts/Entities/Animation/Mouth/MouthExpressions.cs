using Character.Creator;
using Character.Data;
using Reactivity;
using UnityEngine;

public enum MouthExpression
{
	Grin,
	Frown,
	Muse,
	PLACEHOLDER, // The fourth slot is currently empty
	MAX
}

public enum MouthOpenAmount
{
	Closed,
	Ajar,
	Open,
	WideOpen,
	MAX
}

public interface IMouthExpressions
{
	MouthExpression Expression { get; }
	MouthOpenAmount OpenAmount { get; }
}

public class MouthExpressions : ReactiveBehaviour, IMouthExpressions
{
	[SerializeField] CharacterIntId _intId;
	private ICustomizationSelectedDataRepository _dataRepo;

	Computed<int> _intValueComputed;
	Computed<MouthExpression> _expressionComputed;
	Computed<MouthOpenAmount> _openAmountComputed;

	public MouthExpression Expression => _expressionComputed.Val;
	public MouthOpenAmount OpenAmount => _openAmountComputed.Val;


	void Awake()
	{
		_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		_intValueComputed = CreateComputed(ComputeDefaultIntValue);
		_expressionComputed = CreateComputed(ComputeDefaultExpression);
		_openAmountComputed = CreateComputed(ComputeDefaultOpenAmount);
	}

	private int ComputeDefaultIntValue()
	{
		return _dataRepo.GetInt(_intId);
	}

	private MouthExpression ComputeDefaultExpression()
	{
		return (MouthExpression)(_intValueComputed.Val / 4);
	}

	private MouthOpenAmount ComputeDefaultOpenAmount()
	{
		return (MouthOpenAmount)(_intValueComputed.Val % 4);
	}

}

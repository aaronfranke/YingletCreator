using Character.Creator;
using Character.Data;
using Reactivity;
using UnityEngine;

public class MouthExpressionsMutator_Default : ReactiveBehaviour, IMouthExpressionsMutator
{
	[SerializeField] CharacterIntId _intId;

	private ICustomizationSelectedDataRepository _dataRepo;
	Computed<int> _intValueComputed;
	Computed<MouthExpression> _defaultExpressionComputed;
	Computed<MouthOpenAmount> _defaultOpenAmountComputed;

	void Awake()
	{
		_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		_intValueComputed = CreateComputed(ComputeDefaultIntValue);
		_defaultExpressionComputed = CreateComputed(ComputeDefaultExpression);
		_defaultOpenAmountComputed = CreateComputed(ComputeDefaultOpenAmount);
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

	public void Mutate(ref MouthExpression expression, ref MouthOpenAmount openAmount)
	{
		expression = _defaultExpressionComputed.Val;
		openAmount = _defaultOpenAmountComputed.Val;
	}
}

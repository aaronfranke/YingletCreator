using Reactivity;

public class MouthExpressionsMutator_Pose : ReactiveBehaviour, IMouthExpressionsMutator
{
	private IPoseYingDataRepository _dataRepo;
	Computed<int> _intValueComputed;
	Computed<MouthExpression> _defaultExpressionComputed;
	Computed<MouthOpenAmount> _defaultOpenAmountComputed;

	void Awake()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
		_intValueComputed = CreateComputed(ComputeDefaultIntValue);
		_defaultExpressionComputed = CreateComputed(ComputeDefaultExpression);
		_defaultOpenAmountComputed = CreateComputed(ComputeDefaultOpenAmount);
	}

	private int ComputeDefaultIntValue()
	{
		return _dataRepo.YingPoseData.MouthExpressionNum;
	}

	private MouthExpression ComputeDefaultExpression()
	{
		return MouthExpressionsMutator_Default.GetExpressionFromInt(_intValueComputed.Val);
	}

	private MouthOpenAmount ComputeDefaultOpenAmount()
	{
		return MouthExpressionsMutator_Default.GetOpenAmountFromInt(_intValueComputed.Val);
	}

	public void Mutate(ref MouthExpression expression, ref MouthOpenAmount openAmount)
	{
		expression = _defaultExpressionComputed.Val;
		openAmount = _defaultOpenAmountComputed.Val;
	}
}

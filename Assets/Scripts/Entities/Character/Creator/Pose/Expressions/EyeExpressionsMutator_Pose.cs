using Reactivity;


public class EyeExpressionsMutator_Pose : ReactiveBehaviour, IBaseEyeExpressionMutator
{

	private IPoseYingDataRepository _dataRepo;
	private Computed<EyeExpression> _defaultExpressionComputed;

	public EyeExpression DefaultExpression => _defaultExpressionComputed.Val;

	void Awake()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
		_defaultExpressionComputed = CreateComputed(ComputeDefaultExpression);
	}

	private EyeExpression ComputeDefaultExpression()
	{
		return (EyeExpression)(_dataRepo.YingPoseData.EyeExpressionNum);
	}


	public EyeExpression Mutate(EyeExpression input)
	{
		return _defaultExpressionComputed.Val;
	}

}

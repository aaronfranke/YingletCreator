using Character.Creator;
using Character.Data;
using Reactivity;
using UnityEngine;


public class EyeExpressionsMutator_Default : ReactiveBehaviour, IBaseEyeExpressionMutator
{
	[SerializeField] CharacterIntId _intId; // TTODO (probably)

	private ICustomizationSelectedDataRepository _dataRepo;
	private Computed<EyeExpression> _defaultExpressionComputed;

	public EyeExpression DefaultExpression => _defaultExpressionComputed.Val;

	void Awake()
	{
		_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		_defaultExpressionComputed = CreateComputed(ComputeDefaultExpression);
	}

	private EyeExpression ComputeDefaultExpression()
	{
		return (EyeExpression)(_dataRepo.GetInt(_intId));
	}


	public EyeExpression Mutate(EyeExpression input)
	{
		return _defaultExpressionComputed.Val;
	}

}

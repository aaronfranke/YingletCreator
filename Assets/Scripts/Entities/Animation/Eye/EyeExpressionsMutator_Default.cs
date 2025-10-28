using Character.Creator;
using Character.Data;
using Reactivity;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class EyeExpressionsMutator_Default : ReactiveBehaviour, IBaseEyeExpressionMutator
{
	[SerializeField] AssetReferenceT<CharacterIntId> _intIdReference;

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
		return (EyeExpression)(_dataRepo.GetInt(_intIdReference.LoadSync()));
	}


	public EyeExpression Mutate(EyeExpression input)
	{
		return _defaultExpressionComputed.Val;
	}

}

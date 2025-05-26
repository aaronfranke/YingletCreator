using Character.Creator;
using Character.Data;
using Reactivity;
using UnityEngine;

public enum EyeExpression
{
	Normal,
	Squint,
	Closed,
	ClosedHappy,
	Shocked,
	Angry,
	Sad,
	ClosedEnergy
}

/// <summary>
/// Interface that can either add or remove meshes from the set
/// </summary>
public interface IEyeExpressionMutator
{
	/// <summary>
	/// Reflectively called, giving the implementation the opportunity to override the expression
	/// </summary>
	public EyeExpression Mutate(EyeExpression input);
}

public interface IEyeExpressions
{
	public EyeExpression DefaultExpression { get; }
	public EyeExpression CurrentExpression { get; }
}

public class EyeExpressions : ReactiveBehaviour, IEyeExpressions
{
	[SerializeField] CharacterIntId _intId;

	private ICustomizationSelectedDataRepository _dataRepo;
	private IEyeGatherer _eyeGatherer;
	private IEyeExpressionMutator[] _mutators;
	private Computed<EyeExpression> _defaultExpressionComputed;
	private Computed<EyeExpression> _expressionComputed;
	static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");

	public EyeExpression DefaultExpression => _defaultExpressionComputed.Val;
	public EyeExpression CurrentExpression => _expressionComputed.Val;

	void Awake()
	{
		_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		_eyeGatherer = this.GetComponentInParent<IEyeGatherer>();
		_mutators = this.GetComponentsInChildren<IEyeExpressionMutator>();
		_defaultExpressionComputed = CreateComputed(ComputeDefaultExpression);
		_expressionComputed = CreateComputed(ComputeExpression);
		AddReflector(ReflectEyeExpression);
	}

	private EyeExpression ComputeDefaultExpression()
	{
		return (EyeExpression)(_dataRepo.GetInt(_intId));
	}

	private EyeExpression ComputeExpression()
	{
		var expression = _defaultExpressionComputed.Val;
		foreach (var mutator in _mutators)
		{
			expression = mutator.Mutate(expression);
		}
		return expression;
	}

	private void ReflectEyeExpression()
	{
		SetEyesToExpression(_expressionComputed.Val);
	}

	void SetEyesToExpression(EyeExpression eyeExpression)
	{
		foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
		{
			eyeMaterial.SetInteger(EXPRESSION_PROPERTY_ID, (int)eyeExpression);
		}
	}
}

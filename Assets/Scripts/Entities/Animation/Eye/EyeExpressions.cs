using Reactivity;
using UnityEngine;

public enum EyeExpression
{
	Normal,
	Squint,
	Closed,
	// HappyClosed
	// Suprised
	// Angry
	// Sad
	// SadClosed
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


public class EyeExpressions : ReactiveBehaviour
{
	private IEyeGatherer _eyeGatherer;
	private IEyeExpressionMutator[] _mutators;
	static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");

	void Awake()
	{
		_eyeGatherer = this.GetComponentInParent<IEyeGatherer>();
		_mutators = this.GetComponentsInChildren<IEyeExpressionMutator>();
		AddReflector(ReflectEyeExpression);
	}

	private void ReflectEyeExpression()
	{
		var expression = EyeExpression.Normal;

		foreach (var mutator in _mutators)
		{
			expression = mutator.Mutate(expression);
		}

		SetEyesToExpression(expression);
	}

	void SetEyesToExpression(EyeExpression eyeExpression)
	{
		foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
		{
			eyeMaterial.SetInteger(EXPRESSION_PROPERTY_ID, (int)eyeExpression);
		}
	}
}

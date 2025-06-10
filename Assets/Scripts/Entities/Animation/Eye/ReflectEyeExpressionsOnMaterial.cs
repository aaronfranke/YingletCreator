using Reactivity;
using UnityEngine;

internal class ReflectEyeExpressionsOnMaterial : ReactiveBehaviour
{
	private IEyeGatherer _eyeGatherer;
	private IEyeExpressions _eyeExpressions;
	static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");

	private void Awake()
	{
		_eyeGatherer = this.GetComponentInParent<IEyeGatherer>();
		_eyeExpressions = this.GetComponent<IEyeExpressions>();
		AddReflector(ReflectEyeExpression);
	}

	private void ReflectEyeExpression()
	{
		SetEyesToExpression(_eyeExpressions.CurrentExpression);
	}

	void SetEyesToExpression(EyeExpression eyeExpression)
	{
		foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
		{
			eyeMaterial.SetInteger(EXPRESSION_PROPERTY_ID, (int)eyeExpression);
		}
	}
}

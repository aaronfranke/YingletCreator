using Reactivity;
using UnityEngine;

public class ReflectMouthMaterial : ReactiveBehaviour
{
	private IHeadGatherer _headGatherer;
	IMouthExpressions _expressions;


	static readonly int UVMAP_PROPERTY_ID = Shader.PropertyToID("_UVMap");
	static readonly int EXPRESSION_OPEN_PROPERTY_ID = Shader.PropertyToID("_ExpressionOpen");

	private void Awake()
	{
		_headGatherer = this.GetComponent<IHeadGatherer>();
		_expressions = this.GetComponent<IMouthExpressions>();
	}

	void Start()
	{
		AddReflector(ReflectOpen);
	}

	private void ReflectOpen()
	{
		var openAmount = _expressions.OpenAmount;
		var mat = _headGatherer.HeadMaterial;

		mat.SetFloat(UVMAP_PROPERTY_ID, (float)openAmount / ((float)MouthOpenAmount.MAX - 1));
		mat.SetFloat(EXPRESSION_OPEN_PROPERTY_ID, MouthOpenAmount.MAX - 1 - openAmount);
	}
}

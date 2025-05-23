using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMouthMaterial : ReactiveBehaviour
{
	private IHeadGatherer _headGatherer;
	IMouthExpressions _expressions;


	static readonly int UVMAP_PROPERTY_ID = Shader.PropertyToID("_UVMap");
	static readonly int EXPRESSION_PROPERTY_ID = Shader.PropertyToID("_Expression");
	static readonly int EXPRESSION_OPEN_PROPERTY_ID = Shader.PropertyToID("_ExpressionOpen");


	static Dictionary<MouthOpenAmount, float> _defaultUvMap = new()
	{
		{ MouthOpenAmount.Closed, 0 },
		{ MouthOpenAmount.Ajar, 0.255f },
		{ MouthOpenAmount.Open, 0.5f },
		{ MouthOpenAmount.WideOpen, 1f }
	};


	static Dictionary<MouthOpenAmount, float> _museRotationUvMap = new()
	{
		{ MouthOpenAmount.Closed, 0 },
		{ MouthOpenAmount.Ajar, 0.18f },
		{ MouthOpenAmount.Open, 0.35f },
		{ MouthOpenAmount.WideOpen, 0.64f }
	};

	private void Awake()
	{
		_headGatherer = this.GetComponent<IHeadGatherer>();
		_expressions = this.GetComponent<IMouthExpressions>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		var mat = _headGatherer.HeadMaterial;
		var expression = _expressions.Expression;
		var openAmount = _expressions.OpenAmount;

		mat.SetFloat(EXPRESSION_PROPERTY_ID, (float)expression);
		mat.SetFloat(EXPRESSION_OPEN_PROPERTY_ID, MouthOpenAmount.MAX - 1 - openAmount);

		var uvMap = expression == MouthExpression.Muse ? _museRotationUvMap : _defaultUvMap;
		mat.SetFloat(UVMAP_PROPERTY_ID, uvMap[openAmount]);
	}
}

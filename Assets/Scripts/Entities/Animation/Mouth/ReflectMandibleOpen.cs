using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMandibleOpen : ReactiveBehaviour
{
	[SerializeField] Transform _mandibleBone;

	IMouthExpressions _expressions;
	private Quaternion _originalRotation;

	static Dictionary<MouthOpenAmount, float> _defaultRotationMap = new()
	{
		{ MouthOpenAmount.Closed, 0 },
		{ MouthOpenAmount.Ajar, 15 },
		{ MouthOpenAmount.Open, 30 }
	};


	static Dictionary<MouthOpenAmount, float> _museRotationMap = new()
	{
		{ MouthOpenAmount.Closed, 0 },
		{ MouthOpenAmount.Ajar, 11.5f },
		{ MouthOpenAmount.Open, 21.5f }
	};

	private void Awake()
	{
		_expressions = this.GetComponent<IMouthExpressions>();
		_originalRotation = _mandibleBone.localRotation;
	}

	void Start()
	{
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		var expression = _expressions.Expression;
		var openAmount = _expressions.OpenAmount;

		var rotationMap = expression == MouthExpression.Muse ? _museRotationMap : _defaultRotationMap;
		_mandibleBone.localRotation = _originalRotation * Quaternion.Euler(rotationMap[openAmount], 0, 0);
	}
}

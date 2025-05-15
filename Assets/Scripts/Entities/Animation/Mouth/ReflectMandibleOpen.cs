using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMandibleOpen : ReactiveBehaviour
{
	[SerializeField] Transform _mandibleBone;

	IMouthExpressions _expressions;
	private Quaternion _originalRotation;

	static Dictionary<MouthOpenAmount, float> _rotationMap = new()
	{
		{ MouthOpenAmount.Closed, 0 },
		{ MouthOpenAmount.WideOpen, 30 }
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
		var openAmount = _expressions.OpenAmount;
		var rotationAmount = _rotationMap[openAmount];
		_mandibleBone.localRotation = _originalRotation * Quaternion.Euler(rotationAmount, 0, 0);
	}
}

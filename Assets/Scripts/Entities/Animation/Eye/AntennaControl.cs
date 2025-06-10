using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


sealed class Antenna
{
	private readonly Transform _transform;
	private readonly Quaternion _originalRotation;

	public Antenna(Transform transform)
	{
		_transform = transform;
		_originalRotation = transform.localRotation;
	}

	public void SetRotation(float angle)
	{
		_transform.localRotation = _originalRotation * Quaternion.Euler(angle, 0, 0);
	}
}

public class AntennaControl : ReactiveBehaviour
{
	[SerializeField] Transform _rigRoot;
	[SerializeField] EaseSettings _blinkEaseSettings;
	private IEyeExpressions _eyeExpressions;
	private IBlinkTimer _blinkTimer;
	private IEnumerable<Antenna> _antennas;
	Coroutine _blinkCoroutine;

	static readonly Dictionary<EyeExpression, Vector2Int> ANGLE_MAPPING = new()
	{
		{ EyeExpression.Normal,         new Vector2Int(-5,  -30) },
		{ EyeExpression.Squint,         new Vector2Int(-5,  -15) },
		{ EyeExpression.Closed,         new Vector2Int(-10,  -10) },
		{ EyeExpression.ClosedHappy,    new Vector2Int(-5,  -30) },
		{ EyeExpression.Shocked,        new Vector2Int(-5,  -38) },
		{ EyeExpression.Angry,          new Vector2Int(0,  -15) },
		{ EyeExpression.Sad,            new Vector2Int(30,  10) },
		{ EyeExpression.ClosedEnergy,   new Vector2Int(-13,  -13) }
	};

	void Awake()
	{
		_eyeExpressions = this.GetComponent<IEyeExpressions>();
		_blinkTimer = this.GetComponent<IBlinkTimer>();

		var antennaTransforms = TransformUtils.FindChildrenByPrefix(_rigRoot, "antenna_");
		_antennas = antennaTransforms.Select(a => new Antenna(a)).ToArray();
		if (!_antennas.Any()) Debug.LogWarning("Did not find any antennas to control");

		AddReflector(ReflectEyeExpression);

		if (_blinkTimer != null) _blinkTimer.OnBlink += BlinkTimer_OnBlink;
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		if (_blinkTimer != null) _blinkTimer.OnBlink -= BlinkTimer_OnBlink;
	}

	void ReflectEyeExpression()
	{
		var angles = GetFromToAngles();
		SetAntennaRotations(angles.y);
	}

	private void BlinkTimer_OnBlink()
	{
		CoroutineUtils.StartEaseCoroutine(this, ref _blinkCoroutine, _blinkEaseSettings, p =>
		{
			var angles = GetFromToAngles();
			float angle = Mathf.Lerp(angles.x, angles.y, p);
			SetAntennaRotations(angle);
		});
	}

	Vector2 GetFromToAngles()
	{
		var expression = _eyeExpressions.BaseExpression;

		if (!ANGLE_MAPPING.TryGetValue(expression, out var angle))
		{
			Debug.LogError($"No angle mapping found for expression '{expression}'.");
			return Vector2.zero;
		}

		return angle;
	}

	void SetAntennaRotations(float angle)
	{
		foreach (var antenna in _antennas)
		{
			antenna.SetRotation(angle);
		}
	}
}

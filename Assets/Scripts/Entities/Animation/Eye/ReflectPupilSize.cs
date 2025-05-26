using Reactivity;
using UnityEngine;

public class ReflectPupilSize : ReactiveBehaviour
{
	private IEyeGatherer _eyeGatherer;
	private IEyeExpressions _eyeExpressions;

	static float SHOCKED_PUPIL_SIZE = 0.68f;
	static readonly int PUPIL_SCALE_PROPERTY_ID = Shader.PropertyToID("_PupilScale");

	private void Awake()
	{
		_eyeGatherer = this.GetComponent<IEyeGatherer>();
		_eyeExpressions = this.GetComponent<IEyeExpressions>();

		AddReflector(Reflect);
	}

	private void Reflect()
	{
		float size = _eyeExpressions.CurrentExpression == EyeExpression.Shocked
			? SHOCKED_PUPIL_SIZE
			: 1.0f;
		SetPupilsToSize(size);
	}

	void SetPupilsToSize(float size)
	{
		foreach (var eyeMaterial in _eyeGatherer.EyeMaterials)
		{
			eyeMaterial.SetFloat(PUPIL_SCALE_PROPERTY_ID, size);
		}
	}
}

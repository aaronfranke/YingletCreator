using Reactivity;
using System.Linq;
using UnityEngine;

public interface IPupilOffsetMutator
{
	PupilOffsets Mutate(PupilOffsets input);
	bool enabled { get; }
}

public class ReflectPupilOffsetsOnMaterial : ReactiveBehaviour
{
	private IEyeGatherer _eyeGatherer;
	private IPupilOffsetMutator[] _offsetMutators;
	static readonly int PUPIL_OFFSET_X_PROP_ID = Shader.PropertyToID("_PupilOffsetX");
	static readonly int PUPIL_OFFSET_Y_PROP_ID = Shader.PropertyToID("_PupilOffsetY");
	Computed<PupilOffsets> _offsets;

	void Awake()
	{
		_eyeGatherer = this.GetComponentInParent<IEyeGatherer>();
		_offsetMutators = this.GetComponents<IPupilOffsetMutator>().Where(c => c.enabled).ToArray();
		_offsets = CreateComputed(ComputeOffsets);
		AddReflector(ReflectOffsets);
	}


	private PupilOffsets ComputeOffsets()
	{
		PupilOffsets combinedOffset = new PupilOffsets(0, 0, 0);
		foreach (var offsetProvider in _offsetMutators)
		{
			combinedOffset = offsetProvider.Mutate(combinedOffset);
		}
		return combinedOffset;
	}
	private void ReflectOffsets()
	{
		var offsets = _offsets.Val;
		SetMaterial(_eyeGatherer.EyeMaterials[0], offsets.GetLeftEyeOffsets());
		SetMaterial(_eyeGatherer.EyeMaterials[1], offsets.GetRightEyeOffsets());

		void SetMaterial(Material eyeMaterial, Vector2 offset)
		{
			eyeMaterial.SetFloat(PUPIL_OFFSET_X_PROP_ID, offset.x);
			eyeMaterial.SetFloat(PUPIL_OFFSET_Y_PROP_ID, offset.y);
		}
	}

	void Update()
	{
	}
}

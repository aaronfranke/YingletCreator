using UnityEngine;

internal sealed class PupilOffsetMutator_Default : MonoBehaviour, IPupilOffsetMutator
{

	// The pupil starts centered so it can be scaled. Move it to the default eye pos
	public static readonly PupilOffsets InherentOffset = new PupilOffsets(0.151f, .094f, .094f);

	public PupilOffsets Mutate(PupilOffsets input)
	{
		return InherentOffset;
	}
}

using UnityEngine;

internal sealed class PupilOffsetMutator_Pose : MonoBehaviour, IPupilOffsetMutator
{
	private IPoseYingDataRepository _dataRepo;

	private void Awake()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
	}

	public PupilOffsets Mutate(PupilOffsets input)
	{
		return _dataRepo.YingPoseData.PupilData;
	}
}
